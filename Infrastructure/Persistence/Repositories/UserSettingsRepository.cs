using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class UserSettingsRepository : IUserSettingsRepository
    {
        private readonly IApplicationDbContext _context;

        public UserSettingsRepository(IApplicationDbContext context)
        {
            _context = context;
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
