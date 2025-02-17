using Application.Abstractions.Messaging;

namespace Application.Roles.Queries.GetRoleNames
{
    public record GetRoleNamesQuery(int appId): IQuery<string[]>;
}
