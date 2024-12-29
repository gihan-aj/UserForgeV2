using Application.Abstractions.Services;
using Application.Users.Commands.Login;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using SharedKernal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<string>> _roleManager;

        public UserService(RoleManager<IdentityRole<string>> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
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
