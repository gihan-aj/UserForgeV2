using Application.Users.Commands.Login;
using Domain.Users;

namespace Application.Users.Commands.Refresh
{
    public class RefreshUserResponse
    {
        public RefreshUserResponse(User user, string[] roles, string accessToken, string refreshToken)
        {
            User = new BasicUserInfo(user.Id, user.FirstName, user.LastName, roles);
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public string? AccessToken { get; private set; }

        public string? RefreshToken { get; private set; }

        public BasicUserInfo User { get; private set; }

        public BasicUserSettings? UserSettings { get; set; } = null;
    }
}
