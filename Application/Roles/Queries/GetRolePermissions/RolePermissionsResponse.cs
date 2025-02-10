using Domain.Permissions;
using System.Collections.Generic;

namespace Application.Roles.Queries.GetRolePermissions
{
    public record RolePermissionsResponse(
        string Id,
        string Name,
        string? Description,
        bool IsActive,
        List<RolePermissionResponse> Permissions);
}
