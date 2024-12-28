using Domain.Users;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions.Repositories
{
    public interface IRefreshTokenRepository
    {
        void Add(string refreshToken, DateTime expiryDate, User user, string? deviceInfo);
    }
}