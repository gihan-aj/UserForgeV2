using Application.Abstractions.Repositories;
using Application.Data;
using Domain.Users;
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

        public void Add(string refreshToken, DateTime expiryDate, User user, string? deviceInfo)
        {
            var refreshTokenData = new RefreshToken(
                refreshToken,
                expiryDate,
                user.Id,
                deviceInfo);

            _context.RefreshTokens.Add(refreshTokenData);
        }
    }
}
