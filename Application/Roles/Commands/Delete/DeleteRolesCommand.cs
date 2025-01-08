using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.Roles.Commands.Delete
{
    public record DeleteRolesCommand(List<string> Ids, string UserId) : ICommand<List<string>>;
}
