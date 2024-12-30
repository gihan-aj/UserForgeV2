using Application.Abstractions.Messaging;

namespace Application.Users.Commands.ResetPassword
{
    public record ResetPasswordCommand(string UserId, string Token, string NewPassword) : ICommand;
}
