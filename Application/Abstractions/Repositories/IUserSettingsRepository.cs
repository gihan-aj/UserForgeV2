using Domain.Users;
using System.Threading.Tasks;

namespace Application.Abstractions.Repositories
{
    public interface IUserSettingsRepository
    {
        void Add(UserSettings userSettings);
        Task<UserSettings?> GetByUserIdAsync(string userId);
    }
}
