using Application.Abstractions.Messaging;

namespace Application.Roles.Commands.Update
{
    public record UpdateRoleCommand(string RoleId, string RoleName): ICommand;
}
