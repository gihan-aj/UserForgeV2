using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.Roles.Commands.Activate
{
    public record ActivateRolesCommand(List<string> Ids, string UserId) : ICommand<List<string>>;
}
