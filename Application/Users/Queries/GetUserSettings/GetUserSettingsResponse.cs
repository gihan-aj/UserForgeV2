using Domain.UserSettings;

namespace Application.Users.Queries.GetUserSettings
{
    public record GetUserSettingsResponse(UserSetting[] UserSettings);
}
