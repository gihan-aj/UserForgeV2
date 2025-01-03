using Application.Users.Commands.Login;

namespace Application.Users.Commands.Refresh
{
    public record RefreshUserResponse(string AccessToken, string RefreshToken, BasicUserInfo User);
}
