﻿using Domain.Users;
using SharedKernal;
using System.Threading.Tasks;
using System;
using Application.Users.Commands.Login;
using Application.Users.Queries.GetUserApps;
using System.Collections.Generic;
using System.Threading;

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

        Task<Result<User>> LoginAsync(string email, string password, string hashedDeviceIdentifier);

        Task<Result<string[]>> GetRolesAsync(User user);

        Task<Result<User>> GetByIdAsync(string id);

        Task<string?> GeneratePasswordResetTokenAsync(User user);

        Task<Result> ResetPasswordAsync(string userId, string token, string newPassword);

        Task<Result> ChangePasswordAsync(string userId, string currentPassword, string newPassword);

        Task<Result> UpdateUserAsync(
            string userId,
            string firstName,
            string lastName,
            string? phoneNumber,
            DateOnly? dateOfBirth);

        Task<bool> CheckPasswordAsync(User user, string password);

        Task<string?> GenerateChangeEmailTokenAsync(User user, string newEmail);

        Task<Result> ChangeEmailAsync(User user, string newEmail, string token);

        Task<Result<List<UserAppResponse>>> GetUserAppsAsync(string userId, CancellationToken cancellationToken);

    }
}
