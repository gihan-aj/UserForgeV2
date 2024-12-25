using Domain.Users;

namespace Application.Abstractions.Services
{
    public interface ITokenService
    {
        string CreateJwtToken(User user, string[] roles);
        string GenerateRefreshToken();
    }
}
