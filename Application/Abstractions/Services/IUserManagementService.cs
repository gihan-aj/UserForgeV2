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

        Task<Result<List<string>>> ActivateUsers(List<string> ids, CancellationToken cancellationToken);

        Task<Result<List<string>>> DeactivateUsers(List<string> ids, CancellationToken cancellationToken);

        Task<Result<List<string>>> DeleteUsers(List<string> ids, string deletedBy, CancellationToken cancellationToken);
    }
}
