namespace Application.Users.Commands.Refresh
{
    public record RefreshUserRequest(string RefreshToken, string DeviceInfo);
}
