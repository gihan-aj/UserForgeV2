using Application.Abstractions.Messaging;
using System;

namespace Application.Users.Commands.Update
{
    public record UpdateUserCommand(
        string UserId,
        string FirstName,
        string LastName,
        string? PhoneNumber,
        DateOnly? DateOfBirth): ICommand;
}
