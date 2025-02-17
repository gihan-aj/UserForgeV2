using Application.Abstractions.Data;
using Application.Abstractions.Services;
using Domain.Apps;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AppManagementService : IAppManagementService
    {
        private readonly IApplicationDbContext _context;

        public AppManagementService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<App>?> GetAppsToActivateAsync(List<int> ids, CancellationToken cancellationToken)
        {
            return await _context.Apps
                .Where(a => ids.Contains(a.Id) && !a.IsActive)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<List<App>?> GetAppsToDeactivateAsync(List<int> ids, CancellationToken cancellationToken)
        {
            return await _context.Apps
                .Where(a => ids.Contains(a.Id) && a.IsActive)
                .ToListAsync(cancellationToken);
        }
    }
}
