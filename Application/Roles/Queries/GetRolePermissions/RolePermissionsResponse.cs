using Domain.Permissions;
using System.Collections.Generic;

namespace Application.Roles.Queries.GetRolePermissions
{
    public record RolePermissionsResponse(
        string Id,
        string Name,
        string? Description,
        bool IsActive,
        int AppId,
        string AppName,
        List<RolePermissionResponse> Permissions);
}
