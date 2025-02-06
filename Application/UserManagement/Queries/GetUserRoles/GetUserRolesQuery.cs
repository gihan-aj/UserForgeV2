using Application.Abstractions.Messaging;

namespace Application.UserManagement.Queries.GetUserRoles
{
    public record GetUserRolesQuery(string UserId): IQuery<string[]>;
}
