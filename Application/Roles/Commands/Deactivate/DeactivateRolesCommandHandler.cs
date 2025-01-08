using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using SharedKernal;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Roles.Commands.Deactivate
{
    internal sealed class DeactivateRolesCommandHandler : ICommandHandler<DeactivateRolesCommand, List<string>>
    {
        private readonly IRoleManagementService _roleManagementService;

        public DeactivateRolesCommandHandler(IRoleManagementService roleManagementService)
        {
            _roleManagementService = roleManagementService;
        }

        public async Task<Result<List<string>>> Handle(DeactivateRolesCommand request, CancellationToken cancellationToken)
        {
            var result = await _roleManagementService.DeactivateRolesAsync(request.Ids, request.UserId, cancellationToken);
            return result;
        }
    }
}
