

using Application.Abstractions.Services;
using Domain.Users;
using Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using SharedKernal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UsersService : IUsersService
    {
        private readonly RoleManager<IdentityRole<string>> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly TokenSettings _tokenSettings;
        private readonly IEmailService _emailService;

        public UsersService(
            RoleManager<IdentityRole<string>> roleManager, 
            UserManager<User> userManager, 
            IOptions<TokenSettings> tokenSettings, 
            IEmailService emailService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _tokenSettings = tokenSettings.Value;
            _emailService = emailService;
        }

        public async Task<Result<User>> CreateAsync(
            string firstName, 
            string lastName, 
            string email, 
            string? phoneNumber, 
            DateTime? dateOfBirth, 
            string password)
        {
            if (await _userManager.FindByEmailAsync(email) != null)
            {
                return Result.Failure<User>(UserErrors.Conflict.EmailAlreadyExists(email));
            }

            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email,
                EmailConfirmed = false,
                TwoFactorEnabled = false,
                PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber,
                DateOfBirth = dateOfBirth.HasValue
                ? dateOfBirth
                : null,
            };

            var createUserResult = await _userManager.CreateAsync(user, password);
            if (!createUserResult.Succeeded)
            {
                return CreateIdentityError<User>(createUserResult.Errors);
            }

            var addUserRoleResult = await _userManager.AddToRoleAsync(user, Roles.User);
            if (!addUserRoleResult.Succeeded)
            {
                return CreateIdentityError<User>(addUserRoleResult.Errors);
            }

            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var emailSendResult = await _emailService.SendConfirmationEmailAsync(user, emailConfirmationToken);
            if (emailSendResult.IsFailure)
            {
                return emailSendResult;
            }

            return user;
        }

        public async Task<Result> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return Result.Failure(UserErrors.NotFound.User(userId));
            }

            if (user.EmailConfirmed)
            {
                return Result.Failure(UserErrors.Conflict.EmailAlreadyConfirmed(user.Email!));
            }

            var decodedTokenBytes = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded)
            {
                return CreateIdentityError(result.Errors);
            }

            return Result.Success();
        }

        public async Task<Result<User>> ResendEmailConfirmationLink(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user is null)
            {
                return Result.Failure<User>(UserErrors.NotFound.Email(email));
            }

            if (user.EmailConfirmed)
            {
                return Result.Failure<User>(UserErrors.Conflict.EmailAlreadyConfirmed(user.Email!));
            }

            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var emailSendResult = await _emailService.SendConfirmationEmailAsync(user, emailConfirmationToken);
            if (emailSendResult.IsFailure)
            {
                return emailSendResult;
            }

            return user;
        }

        /**
         * Helper methods
         */
        private Result<T> CreateIdentityError<T>(IEnumerable<IdentityError> errors)
        {
            var subErrors = errors
                .Select(identityError => new Error(identityError.Code, identityError.Description))
                .ToList();

            var error = new Error("IdentityError", "One or more validation errors occured.", subErrors);

            return Result.Failure<T>(error);
        }

        private Result CreateIdentityError(IEnumerable<IdentityError> errors)
        {
            var subErrors = errors
                .Select(identityError => new Error(identityError.Code, identityError.Description))
                .ToList();

            var error = new Error("IdentityError", "One or more validation errors occured.", subErrors);

            return Result.Failure(error);
        }
    }
}
