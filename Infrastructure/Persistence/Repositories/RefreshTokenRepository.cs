using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Domain.RefreshTokens;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernal;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IApplicationDbContext _context;

        public RefreshTokenRepository(IApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
        }

        public Task<bool> ExistsAsync(string userId, string deviceInfo)
        {
            return _context.RefreshTokens.AnyAsync(rt => rt.UserId == userId && rt.DeviceIdentifierHash == deviceInfo);
        }

        public void Add(string refreshToken, DateTime expiryDate, User user, string deviceInfo)
        {
            var refreshTokenData = new RefreshToken(
                refreshToken,
                expiryDate,
                user.Id,
                deviceInfo);

            _context.RefreshTokens.Add(refreshTokenData);
        }

        public Task<RefreshToken?> GetAsync(string refreshToken, string deviceInfo)
        {
            return _context.RefreshTokens.FirstOrDefaultAsync(rt =>
                rt.Token == refreshToken && rt.DeviceIdentifierHash == deviceInfo);
        }
        
        public Task<RefreshToken?> GetByUserIdAndDeviceAsync(string userId, string deviceIdentifierHash)
        {
            return _context.RefreshTokens.FirstOrDefaultAsync(rt =>
                rt.UserId == userId && rt.DeviceIdentifierHash == deviceIdentifierHash);
        }

        public async Task<Result> RenewAsync(Guid id, string refreshToken, DateTime expiryDate)
        {
            var token = await _context.RefreshTokens.FindAsync(id);
            if (token is not null)
            {
                token.Renew(refreshToken, expiryDate);
                return Result.Success();
            }

            return UserErrors.Token.MissingRefreshToken;
        }

        public async Task<bool> Validate(string refreshToken, string deviceInfo)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(rt =>
                rt.Token == refreshToken && rt.DeviceIdentifierHash == deviceInfo);

            if(token is null || token.IsExpired || token.IsRevoked)
            {
                return false;
            }

            return true;
        }

        public async Task<Result> RemoveAsync(string userId, string deviceIdentifierHash)
        {
            var tokenData = await _context.RefreshTokens.FirstOrDefaultAsync(rt =>
                rt.UserId == userId && rt.DeviceIdentifierHash == deviceIdentifierHash);

            if (tokenData is null)
            {
                return UserErrors.Conflict.AlreadyLoggedOut(userId);
            }

            _context.RefreshTokens.Remove(tokenData);
            return Result.Success();
        }

        public async Task<Result> LogoutFromAllDevicesAsync(string userId, CancellationToken cancellationToken)
        {
            var tokens = await _context.RefreshTokens.Where(rt => rt.UserId == userId).ToListAsync(cancellationToken);
            if (tokens.Count > 0)
            {
                _context.RefreshTokens.RemoveRange(tokens);
            }
            return Result.Success();
        }
    }
}
