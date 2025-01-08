using Application.Shared.Pagination;
using System.Threading.Tasks;
using System.Threading;
using SharedKernal;
using System.Collections.Generic;
using Application.UserManagement.Queries.GetAll;

namespace Application.Abstractions.Services
{
    public interface IUserManagementService
    {
        Task<PaginatedList<GetAllUsersResponse>> GetAllIUsersAsync(
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            int page,
            int pageSize,
            CancellationToken cancellationToken);

        Task<Result<List<string>>> ActivateUsersAsync(List<string> ids, string modifiedBy, CancellationToken cancellationToken);

        Task<Result<List<string>>> DeactivateUsersAsync(List<string> ids, string modifiedBy, CancellationToken cancellationToken);

        Task<Result<List<string>>> DeleteUsersAsync(List<string> ids, string deletedBy, CancellationToken cancellationToken);
    }
}
