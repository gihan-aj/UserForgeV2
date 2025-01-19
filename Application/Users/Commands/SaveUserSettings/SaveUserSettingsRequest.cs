namespace Application.Users.Commands.SaveUserSettings
{
    public record SaveUserSettingsRequest(
        string Theme,
        int PageSize,
        string DateFormat,
        string TimeFormat);
}
