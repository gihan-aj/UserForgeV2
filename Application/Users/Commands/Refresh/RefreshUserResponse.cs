using Application.Users.Commands.Login;
using Domain.Users;
using Domain.UserSettings;
using System.Linq;

namespace Application.Users.Commands.Refresh
{
    public class RefreshUserResponse
    {
        public RefreshUserResponse(User user, string accessToken, string refreshToken)
        {
            User = new BasicUserInfo(user.Id, user.Email!, user.FirstName, user.LastName);
            Roles = user.UserRoles
                .Select(ur => ur.Role)
                .Select(r => r.Name)
                .ToArray();
            UserSettings = user.UserSettings.ToArray();
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public string? AccessToken { get; private set; }

        public string? RefreshToken { get; private set; }

        public BasicUserInfo User { get; private set; }

        public string?[] Roles { get; private set; }

        public UserSetting[]? UserSettings { get; set; } = null;
    }
}
