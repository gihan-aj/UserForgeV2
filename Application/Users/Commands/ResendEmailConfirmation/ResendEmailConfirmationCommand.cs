using Application.Abstractions.Messaging;
using Domain.Users;

namespace Application.Users.Commands.ResendEmailConfirmation
{
    public record ResendEmailConfirmationCommand(string Email) : ICommand<User>;
}
