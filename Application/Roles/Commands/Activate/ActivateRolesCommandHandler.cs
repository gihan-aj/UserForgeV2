using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using SharedKernal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Roles.Commands.Activate
{
    internal sealed class ActivateRolesCommandHandler : ICommandHandler<ActivateRolesCommand, List<string>>
    {
        private readonly IRoleManagementService _roleManagementService;

        public ActivateRolesCommandHandler(IRoleManagementService roleManagementService)
        {
            _roleManagementService = roleManagementService;
        }

        public async Task<Result<List<string>>> Handle(ActivateRolesCommand request, CancellationToken cancellationToken)
        {
            var result =  await _roleManagementService.ActivateRolesAsync(request.Ids, request.UserId, cancellationToken);
            return result;
        }
    }
}
