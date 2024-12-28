﻿using Domain.Users;
using SharedKernal;
using System.Threading.Tasks;
using System;
using Application.Users.Commands.Login;

namespace Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<Result<User>> CreateAsync(
            string firstName,
            string lastName,
            string email,
            string? phoneNumber,
            DateOnly? dateOfBirth,
            string password);

        Task<Result<string>> GenerateEmailConfirmationTokenAsync(User user);

        Task<Result> ConfirmEmailAsync(string userId, string token);

        Task<Result<User>> FindByEmailAsync(string email);

        Task<Result<User>> LoginAsync(string email, string password);

        Task<Result<string[]>> GetRolesAsync(User user);
    }
}
