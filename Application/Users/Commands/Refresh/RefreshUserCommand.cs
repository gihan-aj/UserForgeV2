using Application.Abstractions.Messaging;

namespace Application.Users.Commands.Refresh
{
    public record RefreshUserCommand(string RefreshToken, string DeviceInfo) : ICommand<RefreshUserResponse>;
}
