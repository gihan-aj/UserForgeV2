using Application.Abstractions.Messaging;

namespace Application.Users.Commands.Login
{
    public record LoginUserCommand(string Email, string Password, string? DeviceInfo): ICommand<LoginUserResponse>;
}
