using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernal;
using System;
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

        public Task<bool> ExistsAsync(string refreshToken, string deviceInfo)
        {
            return _context.RefreshTokens.AnyAsync(rt => rt.Token == refreshToken && rt.DeviceInfo == deviceInfo);
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
            return _context.RefreshTokens.SingleOrDefaultAsync(rt =>
                rt.Token == refreshToken && rt.DeviceInfo == deviceInfo);
        }
        
        public Task<RefreshToken?> GetByUserIdAsync(string userId, string deviceInfo)
        {
            return _context.RefreshTokens.SingleOrDefaultAsync(rt =>
                rt.UserId == userId && rt.DeviceInfo == deviceInfo);
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
            var token = await _context.RefreshTokens.SingleOrDefaultAsync(rt =>
                rt.Token == refreshToken && rt.DeviceInfo == deviceInfo);

            if(token is null || token.IsExpired || token.IsRevoked)
            {
                return false;
            }

            return true;
        }
    }
}
