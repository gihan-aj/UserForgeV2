using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.Permissions.Commands.Deactivate
{
    public record DeactivatePermissionsCommand(List<string> Ids, string ModifiedBy) : ICommand<List<string>>;
}
