using Domain.Users;
using SharedKernal;
using System.Threading.Tasks;

namespace Application.Abstractions.Services
{
    public interface IEmailService
    {
        Task<Result<User>> SendConfirmationEmailAsync(User user, string token);
        Task<Result<User>> SendPasswordResetEmailAsync(User user, string token);
        Task<Result<User>> SendEmailChangeEmailAsync(User user, string token, string newEmail);
    }
}
