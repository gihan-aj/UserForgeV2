using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Domain.Users;
using Domain.UserSettings;
using Microsoft.EntityFrameworkCore;
using SharedKernal;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class UserSettingsRepository : IUserSettingsRepository
    {
        private readonly ApplicationDbContext _context;

        public UserSettingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<User>> GetUserWithSettings(string userId, CancellationToken cancellationToken)
        {
            User? user = await _context.Set<User>()
                .Include(u => u.UserSettings)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if(user is null)
            {
                return Result.Failure<User>(UserErrors.NotFound.User(userId));
            }

            return user;
        }

        public async Task<UserSetting[]> GetUserSettings(string userId, CancellationToken cancellationToken)
        {
            UserSetting[] userSettings = await _context.UserSettings
                .Where(us => us.UserId == userId)
                .ToArrayAsync(cancellationToken);

            return userSettings;
        }

        //public void Add(UserSettings userSettings)
        //{
        //    _context.UserSettings.Add(userSettings);
        //}

        //public async Task<UserSettings?> GetByUserIdAsync(string userId)
        //{
        //    var settings = await _context.UserSettings.FirstOrDefaultAsync(us => us.UserId == userId);
        //    return settings;
        //}

        //public void Update(UserSettings userSettings)
        //{
        //    _context.UserSettings.Update(userSettings);
        //}   
    }
}
