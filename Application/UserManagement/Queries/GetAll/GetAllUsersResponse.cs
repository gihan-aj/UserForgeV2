﻿using System;

namespace Application.UserManagement.Queries.GetAll
{
    public record GetAllUsersResponse(
        string Id,
        string Email,
        string FirstName,
        string LastName,
        string? PhoneNumber,
        DateOnly? DateOfBirth,
        bool EmailConfirmed,
        bool IsActive);
}
