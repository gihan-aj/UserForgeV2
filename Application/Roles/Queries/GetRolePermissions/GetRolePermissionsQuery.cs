using Application.Abstractions.Messaging;

namespace Application.Roles.Queries.GetRolePermissions
{
    public record GetRolePermissionsQuery(string RoleId) : IQuery<RolePermissionsResponse>; 
}
