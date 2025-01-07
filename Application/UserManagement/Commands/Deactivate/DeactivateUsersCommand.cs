using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.UserManagement.Commands.Deactivate
{
    public record DeactivateUsersCommand(List<string> Ids) : ICommand<List<string>>;
}
