using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Domain.Apps;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class AppsRepository : IAppsRepository
    {
        private readonly IApplicationDbContext _context;

        public AppsRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<App?> GetByNameAsync(string name)
        {
            return await _context.Apps.FirstOrDefaultAsync(a => a.Name == name);
        }

        public async Task<bool> AppNameExists(string name)
        {
            return await _context.Apps.AnyAsync(a => a.Name == name);
        }

        public void Add(App app)
        {
            _context.Apps.Add(app);
        }
    }
}
