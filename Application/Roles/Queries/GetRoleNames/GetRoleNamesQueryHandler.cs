using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using SharedKernal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Roles.Queries.GetRoleNames
{
    internal sealed class GetRoleNamesQueryHandler : IQueryHandler<GetRoleNamesQuery, string[]>
    {
        private readonly IRoleManagementService _roleManagementService;

        public GetRoleNamesQueryHandler(IRoleManagementService roleManagementService)
        {
            _roleManagementService = roleManagementService;
        }

        public async Task<Result<string[]>> Handle(GetRoleNamesQuery request, CancellationToken cancellationToken)
        {
            return await _roleManagementService.GetRoleNamesAsync(request.appId, cancellationToken);
        }
    }
}
