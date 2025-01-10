using Application.Abstractions.Services;
using Application.Users.Commands.Login;
using Domain.Roles;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using SharedKernal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Users.UserErrors;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<User>> CreateAsync(
            string firstName,
            string lastName,
            string email,
            string? phoneNumber,
            DateOnly? dateOfBirth,
            string password)
        {
            if (await _userManager.FindByEmailAsync(email) != null)
            {
                return Result.Failure<User>(UserErrors.Conflict.EmailAlreadyExists(email));
            }

            var user = User.Create(firstName, lastName, email);
            user.PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber;
            user.DateOfBirth = dateOfBirth.HasValue
                ? dateOfBirth
                : null;

            var createUserResult = await _userManager.CreateAsync(user, password);
            if (!createUserResult.Succeeded)
            {
                return CreateIdentityError<User>(createUserResult.Errors);
            }

            var addUserRoleResult = await _userManager.AddToRoleAsync(user,Roles.User);
            if (!addUserRoleResult.Succeeded)
            {
                return CreateIdentityError<User>(addUserRoleResult.Errors);
            }

            return Result.Success(user);
        }

        public async Task<Result<string>> GenerateEmailConfirmationTokenAsync(User user)
        {
            if (user.EmailConfirmed)
            {
                return Result.Failure<string>(UserErrors.Conflict.EmailAlreadyConfirmed(user.Email!));
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return token;
        }

        public async Task<Result> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user is null)
            {
                return UserErrors.NotFound.User(userId);
            }

            if (user.EmailConfirmed)
            {
                return UserErrors.Conflict.EmailAlreadyConfirmed(user.Email!);
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

        public async Task<Result<User>> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user is null)
            {
                return Result.Failure<User>(UserErrors.NotFound.Email(email));
            }

            return user;
        }

        public async Task<Result<User>> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user is null)
            {
                return Result.Failure<User>(UserErrors.Validation.InvalidCredentials);
            }

            if (!user.EmailConfirmed)
            {
                return Result.Failure<User>(UserErrors.Authorization.EmailNotConfirmed(email));
            }

            if (!user.IsActive)
            {
                return Result.Failure<User>(UserErrors.Authorization.AccountDeactivated);
            }

            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                return Result.Failure<User>(UserErrors.Validation.InvalidCredentials);
            }

            return Result.Success(user);
        }

        public async Task<Result<string[]>> GetRolesAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if(roles is null)
            {
                return Result.Success<string[]>([]);
            }

            return Result.Success<string[]>(roles.ToArray());
        }

        public async Task<Result<User>> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user is null)
            {
                return Result.Failure<User>(UserErrors.NotFound.User(id));
            }

            return user;
        }

        public async Task<string?> GeneratePasswordResetTokenAsync(User user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        public async Task<Result> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user is null)
            {
                return UserErrors.NotFound.User(userId);
            }

            if (!user.EmailConfirmed)
            {
                return UserErrors.Authorization.EmailNotConfirmed(userId);
            }

            var decodedTokenBytes = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

            var result = await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);
            if(!result.Succeeded)
            {
                return CreateIdentityError(result.Errors);
            }

            return Result.Success();
        }

        public async Task<Result> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return UserErrors.NotFound.User(userId);
            }

            if (!user.EmailConfirmed)
            {
                return UserErrors.Authorization.EmailNotConfirmed(userId);
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
            {
                return CreateIdentityError(result.Errors);
            }

            return Result.Success();
        }

        public async Task<Result> UpdateUserAsync(
            string userId, 
            string firstName, 
            string lastName, 
            string? phoneNumber, 
            DateOnly? dateOfBirth)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return UserErrors.NotFound.User(userId);
            }

            user.Update(firstName, lastName, dateOfBirth, phoneNumber, userId);

            //user.FirstName = firstName;
            //user.LastName = lastName;
            //if (!string.IsNullOrWhiteSpace(phoneNumber))
            //    user.PhoneNumber = phoneNumber;

            //if (dateOfBirth.HasValue)
            //{
            //    user.DateOfBirth = dateOfBirth;
            //}

            var updatedResult = await _userManager.UpdateAsync(user);
            if (!updatedResult.Succeeded)
            {
                return CreateIdentityError(updatedResult.Errors);
            }

            return Result.Success();
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            return isPasswordValid;
        }

        public async Task<string?> GenerateChangeEmailTokenAsync(User user, string newEmail)
        {
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);

            return token;
        }

        public async Task<Result> ChangeEmailAsync(User user, string newEmail, string token)
        {
            var decodedTokenBytes = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

            var emailChangeResult = await _userManager.ChangeEmailAsync(user, newEmail, decodedToken);
            if (!emailChangeResult.Succeeded)
            {
                return CreateIdentityError(emailChangeResult.Errors);
            }

            user.NormalizedEmail = _userManager.NormalizeEmail(user.Email);
            user.UserName = newEmail;
            user.EmailConfirmed = true;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return CreateIdentityError(updateResult.Errors);
            }

            return Result.Success();
        }

        private Result<T> CreateIdentityError<T>(IEnumerable<IdentityError> errors)
        {
            var subErrors = errors
                .Select(identityError => new Error(identityError.Code, identityError.Description))
                .ToList();

            var error = new Error("IdentityError", "A problem occured during operation.", subErrors);

            return Result.Failure<T>(error);
        }

        private Result CreateIdentityError(IEnumerable<IdentityError> errors)
        {
            var subErrors = errors
                .Select(identityError => new Error(identityError.Code, identityError.Description))
                .ToList();

            var error = new Error("IdentityError", "A problem occured during operation.", subErrors);

            return Result.Failure(error);
        }
    }
}
