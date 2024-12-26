using Application.Abstractions.Messaging;

namespace Application.Users.Commands.ConfirmEmail
{
    public record ConfirmEmailCommand(string UserId, string Token): ICommand;
}
