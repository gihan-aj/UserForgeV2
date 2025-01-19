using Domain.Users;
using Domain.UserSettings;
using SharedKernal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions.Repositories
{
    public interface IUserSettingsRepository
    {
        //void Add(UserSettings userSettings);
        //Task<UserSettings?> GetByUserIdAsync(string userId);
        //void Update(UserSettings userSettings);
        Task<Result<User>> GetUserWithSettings(string userId, CancellationToken cancellationToken);
        Task<UserSetting[]> GetUserSettings(string userId, CancellationToken cancellationToken);
    }
}
