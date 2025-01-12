using Application.Permissions.Queries.GetAll;
using Application.Shared.Pagination;
using Domain.Permissions;
using Domain.Roles;
using SharedKernal;
using System.Collections.Generic;
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

        Task<Result> AssignRolePermissionsAsync(
            Role role,
            List<string> permissionIds,
            string modifiedBy,
            CancellationToken cancellationToken);
    }
}
