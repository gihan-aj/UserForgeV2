using Application.Abstractions.Services;
using Domain.Roles;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using SharedKernal;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Infrastructure.Services
{
    public class RoleManagementService : IRoleManagementService
    {
        private readonly RoleManager<IdentityRole<string>> _roleManager;
        public RoleManagementService(RoleManager<IdentityRole<string>> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Result<string>> CreateAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if(roleExists)
            {
                return Result.Failure<string>(RoleErrors.Conflict.RoleNameAlreadyExists(roleName));
            }

            var role = new IdentityRole(roleName);

            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return CreateIdentityError<string>(result.Errors);
            }

            return role.Id;
        }

        public async Task<Result> UpdateAsync(string roleId, string roleName)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if(role is null)
            {
                return RoleErrors.NotFound.Role(roleId);
            }

            role.Name = roleName;
            role.NormalizedName = _roleManager.NormalizeKey(roleName);

            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                return CreateIdentityError(result.Errors);
            }

            return Result.Success();
        }

        private Result CreateIdentityError(IEnumerable<IdentityError> errors)
        {
            var subErrors = errors
                .Select(identityError => new Error(identityError.Code, identityError.Description))
                .ToList();

            var error = new Error("IdentityError", "One or more validation errors occured.", subErrors);

            return Result.Failure(error);
        }

        private Result<T> CreateIdentityError<T>(IEnumerable<IdentityError> errors)
        {
            var subErrors = errors
                .Select(identityError => new Error(identityError.Code, identityError.Description))
                .ToList();

            var error = new Error("IdentityError", "One or more validation errors occured.", subErrors);

            return Result.Failure<T>(error);
        }
    }
}
