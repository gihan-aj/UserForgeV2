using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.UserManagement.Commands.Activate
{
    public record ActivateUsersCommand(List<string> Ids, string UserId) : ICommand<List<string>>;
}
