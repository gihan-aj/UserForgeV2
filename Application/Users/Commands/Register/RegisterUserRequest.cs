using System;

namespace Application.Users.Commands.Register
{
    public record RegisterUserRequest(
        string FirstName,
        string LastName,
        string Email,
        string? PhoneNumber,
        DateTime? DateOfBirth,
        string Password
        );
}
