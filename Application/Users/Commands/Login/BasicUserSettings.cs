namespace Application.Users.Commands.Login
{
    public record BasicUserSettings(
        string Theme,
        string Language,
        string DateFormat,
        string TimeFormat,
        string TimeZone);
}
