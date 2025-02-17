using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.Apps.Commands.Deactivate
{
    public record DeactivateAppsCommand(List<int> Ids, string ModifiedBy) : ICommand<List<string>>;
}
