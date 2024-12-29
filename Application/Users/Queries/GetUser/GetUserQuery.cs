using Application.Abstractions.Messaging;

namespace Application.Users.Queries.GetUser
{
    public record GetUserQuery(string UserId) : IQuery<GetUserResponse>;
}
