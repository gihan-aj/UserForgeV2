using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.UserManagement.Commands.Activate
{
    public record ActivateUsersCommand(List<string> Ids) : ICommand<List<string>>;
}
