using Application.Abstractions.Messaging;

namespace Application.Users.Commands.ChangeEmail
{
    public record ChangeEmailCommand(string UserId, string Token, string NewEmail, string Password) : ICommand;
}
