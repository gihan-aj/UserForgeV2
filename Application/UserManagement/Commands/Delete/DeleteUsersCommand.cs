using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.UserManagement.Commands.Delete
{
    public record DeleteUsersCommand(List<string> UserIds, string DeletedBy) : ICommand<List<string>>;
}
