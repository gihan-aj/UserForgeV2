using Application.Abstractions.Messaging;

namespace Application.Permissions.Commands.Create
{
    public record CreatePermissionCommand(string Name, string? Description, string CreatedBy) : ICommand<string>;
}
