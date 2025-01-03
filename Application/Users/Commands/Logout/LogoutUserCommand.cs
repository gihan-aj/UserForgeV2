using Application.Abstractions.Messaging;

namespace Application.Users.Commands.Logout
{
    public record LogoutUserCommand(string UserId, string DeviceIdentifier) : ICommand;
}
