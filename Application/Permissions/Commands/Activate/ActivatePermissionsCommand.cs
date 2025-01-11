using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.Permissions.Commands.Activate
{
    public record ActivatePermissionsCommand(List<string> Ids, string ModifiedBy) : ICommand<List<string>>;
}
