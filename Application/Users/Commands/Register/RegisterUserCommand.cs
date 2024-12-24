using Application.Abstractions.Messaging;
using Domain.Users;
using SharedKernal;
using System;

namespace Application.Users.Commands.Register
{
    public record RegisterUserCommand(
        string FirstName,
        string LastName,
        string Email,
        string? PhoneNumber,
        DateTime? DateOfBirth,
        string Password
        ) : ICommand<User>;
}
