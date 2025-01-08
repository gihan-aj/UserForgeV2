using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using SharedKernal;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Roles.Commands.Delete
{
    internal sealed class DeleteRolesCommandHandler : ICommandHandler<DeleteRolesCommand, List<string>>
    {
        private readonly IRoleManagementService _roleManagementService;

        public DeleteRolesCommandHandler(IRoleManagementService roleManagementService)
        {
            _roleManagementService = roleManagementService;
        }

        public async Task<Result<List<string>>> Handle(DeleteRolesCommand request, CancellationToken cancellationToken)
        {
            var result = await _roleManagementService.DeleteRolesAsync(request.Ids, request.UserId, cancellationToken);
            return result;
        }
    }
}
