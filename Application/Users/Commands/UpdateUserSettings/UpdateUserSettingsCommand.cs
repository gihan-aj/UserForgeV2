using Application.Abstractions.Messaging;

namespace Application.Users.Commands.UpdateUserSettings
{
    public record UpdateUserSettingsCommand(
        string UserId,
        string Theme,
        string Language,
        string DateFormat,
        string TimeFormat,
        string TimeZone,
        bool NotificationsEnabled,
        bool EmailNotification,
        bool SmsNotification) : ICommand;
}
