using Application.Abstractions.Messaging;

namespace Application.Roles.Commands.Create
{
    public record CreateRoleCommand(string RoleName): ICommand<string>;
}
