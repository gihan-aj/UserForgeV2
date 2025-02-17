using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.Apps.Commands.Activate
{
    public record ActivateAppsCommand(List<int> Ids, string ModifiedBy) : ICommand<List<string>>;
}
