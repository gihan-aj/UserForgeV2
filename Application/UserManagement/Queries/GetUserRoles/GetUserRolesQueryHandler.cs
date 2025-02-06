using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using SharedKernal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserManagement.Queries.GetUserRoles
{
    internal sealed class GetUserRolesQueryHandler : IQueryHandler<GetUserRolesQuery, string[]>
    {
        private readonly IUserManagementService _userManagementService;
        public GetUserRolesQueryHandler(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }
        public async Task<Result<string[]>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            var result = await _userManagementService.GetUserRolesAsync(request.UserId);
            return result;
        }
    }
}
