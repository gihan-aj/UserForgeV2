using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using SharedKernal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Roles.Commands.Create
{
    internal sealed class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, string>
    {
        private readonly IRoleManagementService _roleManagementService;

        public CreateRoleCommandHandler(IRoleManagementService roleManagementService)
        {
            _roleManagementService = roleManagementService;
        }

        public async Task<Result<string>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _roleManagementService.CreateAsync(
                request.RoleName, 
                request.Description, 
                request.AppId, 
                request.UserId);
            return result;
        }
    }
}
