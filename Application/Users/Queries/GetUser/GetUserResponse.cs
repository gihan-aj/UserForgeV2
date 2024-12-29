using System;

namespace Application.Users.Queries.GetUser
{
    public record GetUserResponse(
        string Id,
        string Email,
        string FirstName,
        string LastName,
        string? PhoneNumber,
        DateOnly? DateOfBirth);
}
