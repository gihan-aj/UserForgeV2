using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.UserManagement.Commands.AssignRoles
{
    public record AssignUserRolesCommand(string UserId, List<string> RoleNames, string ModifiedBy) : ICommand<List<string>>;
}
