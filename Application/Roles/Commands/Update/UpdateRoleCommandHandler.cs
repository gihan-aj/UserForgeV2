using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using SharedKernal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Roles.Commands.Update
{
    internal sealed class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand>
    {
        private readonly IRoleManagementService _roleManagementService;

        public UpdateRoleCommandHandler(IRoleManagementService roleManagementService)
        {
            _roleManagementService = roleManagementService;
        }

        public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _roleManagementService.UpdateAsync(
                request.RoleId, 
                request.RoleName, 
                request.Description, 
                request.UserId);

            return result;
        }
    }
}
