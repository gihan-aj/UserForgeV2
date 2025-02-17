using Application.Apps.Queries.GetPaginated;
using Application.Shared.Pagination;
using Domain.Apps;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions.Repositories
{
    public interface IAppsRepository
    {
        Task<App?> GetByNameAsync(string name);
        Task<bool> AppNameExists(string name);
        void Add(App app);
        Task<App?> GetByIdAsync(int id);
        Task<PaginatedList<PaginatedAppResponse>> GetPaginatedAppListAsync(
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            int page,
            int pageSize,
            CancellationToken cancellationToken);

    }
}
