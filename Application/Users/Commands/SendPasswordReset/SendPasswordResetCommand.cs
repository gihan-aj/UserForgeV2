using Application.Abstractions.Messaging;

namespace Application.Users.Commands.SendPasswordReset
{
    public record SendPasswordResetCommand(string Email) : ICommand;
}
