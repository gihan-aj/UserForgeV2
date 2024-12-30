using System;

namespace Application.Users.Commands.Update
{
    public record UpdateUserRequest(
        string FirstName, 
        string LastName, 
        string? PhoneNumber, 
        DateOnly? DateOfBirth);
}
