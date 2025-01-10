using Application.Abstractions.Messaging;

namespace Application.Permissions.Commands.Update
{
    public record UpdatePermissionCommand(string Id, string Name, string? Description, string ModifiedBy) : ICommand;
}
