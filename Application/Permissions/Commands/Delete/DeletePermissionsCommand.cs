using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.Permissions.Commands.Delete
{
    public record DeletePermissionsCommand(List<string> Ids, string ModifiedBy) : ICommand<List<string>>;
}
