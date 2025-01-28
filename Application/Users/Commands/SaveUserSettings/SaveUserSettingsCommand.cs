using Application.Abstractions.Messaging;

namespace Application.Users.Commands.SaveUserSettings
{
    public record SaveUserSettingsCommand(
        SaveUserSettingsRequest[] UserSettings,
        string UserId) : ICommand;
}
