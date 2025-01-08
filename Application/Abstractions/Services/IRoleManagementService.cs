﻿
using Application.Roles.Queries.GetAll;
using Application.Shared.Pagination;
using SharedKernal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions.Services
{
    public interface IRoleManagementService
    {
        Task<Result<string>> CreateAsync(string roleName, string description, string userId);

        Task<Result> UpdateAsync(string roleId, string roleName, string description, string userId);

        Task<PaginatedList<GetAllRolesResponse>> GetAllRolesAsync(
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            int page,
            int pageSize,
            CancellationToken cancellationToken);

        Task<Result<List<string>>> ActivateRoles(List<string> ids, string modifiedBy, CancellationToken cancellationToken);

        Task<Result<List<string>>> DeactivateRoles(List<string> ids, string modifiedBy, CancellationToken cancellationToken);
    }
}
