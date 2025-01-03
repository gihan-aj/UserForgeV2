using Domain.Users;

namespace Application.Users.Commands.Login
{
    public class LoginUserResponse
    {
        public LoginUserResponse(User user, string[] roles, string accessToken, string refreshToken)
        {
            User = new BasicUserInfo(user.Id, user.FirstName, user.LastName, roles);
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public string? AccessToken { get; private set; } = null;

        public string? RefreshToken { get; private set; } = null;

        public BasicUserInfo User {  get; private set; }

        public BasicUserSettings? UserSettings { get; set; } = null;
    }
}
