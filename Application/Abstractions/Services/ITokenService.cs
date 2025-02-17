using Domain.Users;

namespace Application.Abstractions.Services
{
    public interface ITokenService
    {
        string CreateJwtToken(User user, int appId, string[] roles);
        string GenerateRefreshToken();
        string Hash(string input);
    }
}
