
using Application.Roles.Queries.GetAll;
using Application.Shared.Pagination;
using SharedKernal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions.Services
{
    public interface IRoleManagementService
    {
        Task<Result<string>> CreateAsync(string roleName, string description, string userId);

        Task<Result> UpdateAsync(string roleId, string roleName);

        Task<PaginatedList<GetAllRolesResponse>> GetAllRolesAsync(
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            int page,
            int pageSize,
            CancellationToken cancellationToken);
    }
}
