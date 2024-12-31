using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Application.Shared.Pagination;
using SharedKernal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetAll
{
    internal sealed class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, PaginatedList<GetAllUsersResponse>>
    {
        private readonly IUserManagementService _userManagementService;

        public GetAllUsersQueryHandler(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        public async Task<Result<PaginatedList<GetAllUsersResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManagementService.GetAllIUsersAsync(
                request.SearchTerm,
                request.SortColumn,
                request.SortOrder,
                request.Page,
                request.PageSize,
                cancellationToken);

            return users;
        }
    }
}
