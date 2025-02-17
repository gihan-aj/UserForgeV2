using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.Users.Queries.GetUserPermissions
{
    public record GetUserPermissionsQuery(string UserId, int AppId): IQuery<List<string>>;
}
