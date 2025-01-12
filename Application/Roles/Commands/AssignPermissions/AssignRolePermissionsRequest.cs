using System.Collections.Generic;

namespace Application.Roles.Commands.AssignPermissions
{
    public record AssignRolePermissionsRequest(string RoleId, List<string> PermissionIds);
}
