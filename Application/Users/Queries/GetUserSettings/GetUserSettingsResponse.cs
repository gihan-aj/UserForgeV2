namespace Application.Users.Queries.GetUserSettings
{
    public record GetUserSettingsResponse(
        string Theme,
        string Language,
        string DateFormat,
        string TimeFormat,
        string TimeZone,
        bool NotificationsEnabled,
        bool EmailNotification,
        bool SmsNotification);
}
