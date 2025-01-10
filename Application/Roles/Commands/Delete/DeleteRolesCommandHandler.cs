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
        private readonly IUserManagementService _userManagementService;

        public DeleteRolesCommandHandler(IRoleManagementService roleManagementService, IUserManagementService userManagementService)
        {
            _roleManagementService = roleManagementService;
            _userManagementService = userManagementService;
        }

        public async Task<Result<List<string>>> Handle(DeleteRolesCommand request, CancellationToken cancellationToken)
        {
            var roleNames = new List<string>();
            foreach (var roleId in request.Ids)
            {
                var role = await _roleManagementService.GetRoleById(roleId);
                if (role is not null)
                {
                    roleNames.Add(role.Name!);
                }
            }

            foreach (var role in roleNames)
            {
                var deleteUserRolesResult = await _userManagementService.DeleteUserRolesByRoleNameAsync(role, cancellationToken);
                if (deleteUserRolesResult.IsFailure)
                {
                    return Result.Failure<List<string>>(deleteUserRolesResult.Error);
                }
            }

            var result = await _roleManagementService.DeleteRolesAsync(request.Ids, request.UserId, cancellationToken);
            
            return result;
        }
    }
}
