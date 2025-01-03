using Application.Abstractions.Messaging;

namespace Application.Users.Queries.GetUserSettings
{
    public record GetUserSettingsQuery(string UserId) : IQuery<GetUserSettingsResponse>;
}
