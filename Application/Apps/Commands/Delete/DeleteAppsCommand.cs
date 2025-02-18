using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.Apps.Commands.Delete
{
    public record DeleteAppsCommand(List<int> Ids, string DeletedBy): ICommand<List<string>>;
}
