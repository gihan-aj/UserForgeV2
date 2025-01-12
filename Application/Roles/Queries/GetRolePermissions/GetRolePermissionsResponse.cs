using Application.Permissions.Queries.GetAll;
using System.Collections.Generic;

namespace Application.Roles.Queries.GetRolePermissions
{
    public record GetRolePermissionsResponse(
        string Id,
        string Name,
        string? Description,
        bool IsActive,
        List<GetAllPermissionsResponse> Permissions);
}
