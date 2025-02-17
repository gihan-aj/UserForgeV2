using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Application.Apps.Queries.GetPaginated;
using Application.Shared.Pagination;
using Domain.Apps;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
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

        public async Task<App?> GetByIdAsync(int id)
        {
            return await _context.Apps.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<PaginatedList<PaginatedAppResponse>> GetPaginatedAppListAsync(
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            int page,
            int pageSize,
            CancellationToken cancellationToken)
        {
            IQueryable<App> appsQuery = _context.Apps.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                appsQuery = appsQuery
                    .Where(a => a.Name.Contains(searchTerm));
            }

            if(sortOrder?.Trim().ToLower() == "desc")
            {
                appsQuery = appsQuery
                    .OrderByDescending(GetSortProperty(sortColumn));
            }
            else
            {
                appsQuery = appsQuery
                    .OrderBy(GetSortProperty(sortColumn));
            }

            var appsResponseQuery = appsQuery
                .Select(a => new PaginatedAppResponse(
                    a.Id,
                    a.Name,
                    a.Description,
                    a.BaseUrl,
                    a.IsActive));

            var apps = await PaginatedList<PaginatedAppResponse>.CreateAsync(
                appsResponseQuery,
                page,
                pageSize,
                cancellationToken);

            return apps;
        }

        private static Expression<Func<App, object>> GetSortProperty(string? sortColumn)
        {
            return sortColumn?.ToLower() switch
            {
                "isActive" => app => app.IsActive,
                "name" => app => app.Name,
                "id" => app => app.Id,
                _ => app => app.Id
            };
        }
    }
}
