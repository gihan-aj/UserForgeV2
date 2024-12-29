using Application.Abstractions.Messaging;

namespace Application.Users.Commands.Refresh
{
    public record RefreshUserCommand(string RefreshToken, string DeviceIdentifier) : ICommand<RefreshUserResponse>;
}
