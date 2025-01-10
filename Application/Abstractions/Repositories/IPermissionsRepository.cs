using Application.Permissions.Queries.GetAll;
using Application.Shared.Pagination;
using Domain.Permissions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions.Repositories
{
    public interface IPermissionsRepository
    {
        void Add(Permission permission);

        void Remove(Permission permission);

        void Update(Permission permission);

        Task<bool> NameExistsAsync(string permissionName);

        Task<Permission?> GetByIdAsync(string id);

        Task<PaginatedList<GetAllPermissionsResponse>> GetAllAsync(
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            int page,
            int pageSize,
            CancellationToken cancellationToken);
    }
}
