using Application.Abstractions.Messaging;

namespace Application.Roles.Commands.Create
{
    public record CreateRoleCommand(string RoleName, string Description, int AppId, string UserId) : ICommand<string>;
}
