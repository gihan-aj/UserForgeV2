namespace Application.Users.Commands.SaveUserSettings
{
    public record SaveUserSettingsRequest(
        string Key,
        string Value,
        string? DataType);
}
