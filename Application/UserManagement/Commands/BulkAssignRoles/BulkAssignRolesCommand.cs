using Application.Abstractions.Messaging;

namespace Application.UserManagement.Commands.BulkAssignRoles
{
    public record BulkAssignRolesCommand(string[] UserIds, string[] RoleNames, string AssignedBy): ICommand;
}
