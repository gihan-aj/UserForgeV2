using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.Roles.Commands.Deactivate
{
    public record DeactivateRolesCommand(List<string> Ids, string UserId) : ICommand<List<string>>;
}
