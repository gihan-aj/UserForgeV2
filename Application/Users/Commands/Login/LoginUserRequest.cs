namespace Application.Users.Commands.Login
{
    public record LoginUserRequest(string Email, string Password, string DeviceInfo);
}
