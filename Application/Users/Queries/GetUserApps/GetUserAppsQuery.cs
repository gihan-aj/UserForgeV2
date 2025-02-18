using Application.Abstractions.Messaging;
using Application.Users.Queries.GetUser;
using System.Collections.Generic;

namespace Application.Users.Queries.GetUserApps
{
    public record GetUserAppsQuery(string UserId) : IQuery<List<UserAppResponse>>;
}
