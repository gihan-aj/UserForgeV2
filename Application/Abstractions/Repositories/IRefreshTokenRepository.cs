using Domain.Users;
using SharedKernal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<bool> ExistsAsync(string userId, string deviceInfo);

        void Add(string refreshToken, DateTime expiryDate, User user, string deviceInfo);

        Task<RefreshToken?> GetAsync(string refreshToken, string deviceInfo);

        Task<RefreshToken?> GetByUserIdAndDeviceAsync(string userId, string deviceInfo);

        Task<Result> RenewAsync(Guid id, string refreshToken, DateTime expiryDate);

        Task<bool> Validate(string refreshToken, string deviceInfo);

        Task<Result> RemoveAsync(string userId, string deviceIdentifierHash);
    }
}