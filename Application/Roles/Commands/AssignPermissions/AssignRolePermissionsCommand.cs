using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.Roles.Commands.AssignPermissions
{
    public record AssignRolePermissionsCommand(string RoleId, List<string> PermissionIds, string ModifiedBy) : ICommand;
}
