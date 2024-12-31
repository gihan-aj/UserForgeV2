using Application.Shared.Pagination;
using Application.Users.Queries.GetAll;
using System.Threading.Tasks;
using System.Threading;
using SharedKernal;
using System.Collections.Generic;

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
    }
}
