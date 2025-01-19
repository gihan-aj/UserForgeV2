using Application.Abstractions.Messaging;

namespace Application.Users.Commands.SaveUserSettings
{
    public record SaveUserSettingsCommand(
        string Theme,
        int PageSize,
        string DateFormat,
        string TimeFormat,
        string UserId) : ICommand;
}
