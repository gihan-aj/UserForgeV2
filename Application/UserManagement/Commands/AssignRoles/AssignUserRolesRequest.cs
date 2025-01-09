using System.Collections.Generic;

namespace Application.UserManagement.Commands.AssignRoles
{
    public record AssignUserRolesRequest(string UserId, List<string> RoleNames);
}
