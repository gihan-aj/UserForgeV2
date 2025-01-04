namespace Application.Users.Commands.UpdateUserSettings
{
    public record UpdateUserSettingsRequest(
        string Theme,
        string Language,
        string DateFormat,
        string TimeFormat,
        string TimeZone,
        bool NotificationsEnabled,
        bool EmailNotification,
        bool SmsNotification);
}
