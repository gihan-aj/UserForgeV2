using Application.Abstractions.Messaging;

namespace Application.Users.Commands.ChangePassword
{
    public record ChangePasswordCommand(
        string UserId, 
        string CurrentPassword, 
        string NewPassword) : ICommand;
}
