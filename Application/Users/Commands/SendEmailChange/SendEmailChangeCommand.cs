using Application.Abstractions.Messaging;

namespace Application.Users.Commands.SendEmailChange
{
    public record SendEmailChangeCommand(string UserId, string NewEmail, string Password) : ICommand;
}
