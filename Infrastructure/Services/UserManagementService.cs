using Application.Abstractions.Services;
using Application.Shared.Pagination;
using Application.UserManagement.Queries.GetAll;
using Application.Users.Queries.GetUser;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<User> _userManager;

        public UserManagementService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<PaginatedList<GetAllUsersResponse>> GetAllIUsersAsync(
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            int page,
            int pageSize,
            CancellationToken cancellationToken)
        {
            IQueryable<User> usersQuery = _userManager.Users.AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                usersQuery = usersQuery
                    .Where(u =>
                    u.FirstName.Contains(searchTerm) ||
                    u.LastName.Contains(searchTerm) ||
                    u.Email!.Contains(searchTerm));
            }

            // Sorting
            if (sortOrder?.ToLower() == "desc")
            {
                usersQuery = usersQuery
                    .OrderByDescending(GetSortProperty(sortColumn));
            }
            else
            {
                usersQuery = usersQuery
                    .OrderBy(GetSortProperty(sortColumn));
            }

            // Selecting
            var usersResponsQuery = usersQuery
                .Select(u => new GetAllUsersResponse(
                    u.Id,
                    u.Email!,
                    u.FirstName,
                    u.LastName,
                    u.PhoneNumber,
                    u.DateOfBirth,
                    u.EmailConfirmed,
                    u.IsActive));

            var users = await PaginatedList<GetAllUsersResponse>.CreateAsync(
                usersResponsQuery,
                page,
                pageSize,
                cancellationToken);

            return users;
        }

        public async Task<Result<List<string>>> ActivateUsersAsync(List<string> ids, string modifiedBy, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users
                .Where(u => ids.Contains(u.Id))
                .ToListAsync(cancellationToken);

            var activatedUsers = new List<string>();

            if (users.Count == 0)
            {
                if (ids.Count == 1)
                {
                    return Result.Failure<List<string>>(UserErrors.NotFound.User(ids[0]));
                }

                return Result.Failure<List<string>>(UserErrors.NotFound.Users);
            }
            else
            {
                foreach (var user in users)
                {
                    if (!user.IsActive)
                    {
                        user.Activate(modifiedBy);
                        var updateResult = await _userManager.UpdateAsync(user);
                        if (!updateResult.Succeeded)
                        {
                            return CreateIdentityError<List<string>>(updateResult.Errors);
                        }

                        activatedUsers.Add(user.Id);
                    }
                }

                return activatedUsers;
            }
        }
        
        public async Task<Result<List<string>>> DeactivateUsersAsync(List<string> ids, string modifiedBy, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users
                .Where(u => ids.Contains(u.Id))
                .ToListAsync(cancellationToken);

            var deactivatedUsers = new List<string>();

            if (users.Count == 0)
            {
                if(ids.Count == 1)
                {
                    return Result.Failure<List<string>>(UserErrors.NotFound.User(ids[0]));
                }

                return Result.Failure<List<string>>(UserErrors.NotFound.Users);
            }
            else
            {
                foreach (var user in users)
                {
                    if (user.IsActive)
                    {
                        user.Deactivate(modifiedBy);
                        var updateResult = await _userManager.UpdateAsync(user);
                        if (!updateResult.Succeeded)
                        {
                            return CreateIdentityError<List<string>>(updateResult.Errors);
                        }

                        deactivatedUsers.Add(user.Id);
                    }
                }

                return deactivatedUsers;
            }
        }

        public async Task<Result<List<string>>> AssignUserRolesAsync(string userId, List<string> roleNames, string modifiedBy, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return Result.Failure<List<string>>(UserErrors.NotFound.User(userId));
            }
            var roles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = roleNames.Except(roles).ToList();
            var rolesToRemove = roles.Except(roleNames).ToList();
            var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
            if (!addResult.Succeeded)
            {
                return CreateIdentityError<List<string>>(addResult.Errors);
            }
            var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!removeResult.Succeeded)
            {
                return CreateIdentityError<List<string>>(removeResult.Errors);
            }

            user.UserRolesModified(modifiedBy);
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return CreateIdentityError<List<string>>(updateResult.Errors);
            }

            return roles.Except(rolesToRemove).Concat(rolesToAdd).ToList();
        }

        public async Task<Result<List<string>>> DeleteUsersAsync(List<string> ids, string deletedBy, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users
                .Where(u => ids.Contains(u.Id))
                .ToListAsync(cancellationToken);

            var deletedUsers = new List<string>();
            if (users.Count == 0)
            {
                if (ids.Count == 1)
                {
                    return Result.Failure<List<string>>(UserErrors.NotFound.User(ids[0]));
                }
                return Result.Failure<List<string>>(UserErrors.NotFound.Users);
            }
            else
            {
                foreach (var user in users)
                {
                    user.DeletedBy = deletedBy;
                    var deleteResult = await _userManager.DeleteAsync(user);
                    if (!deleteResult.Succeeded)
                    {
                        return CreateIdentityError<List<string>>(deleteResult.Errors);
                    }
                    var deletedUserRoles = await _userManager.GetRolesAsync(user);
                    if (deletedUserRoles.Count > 0)
                    {
                        var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, deletedUserRoles);
                        if (!removeRolesResult.Succeeded)
                        {
                            return CreateIdentityError<List<string>>(removeRolesResult.Errors);
                        }
                    }

                    deletedUsers.Add(user.Id);
                }
                return deletedUsers;
            }
        }

        public async Task<Result> DeleteUserRolesByRoleNameAsync(string roleName, CancellationToken cancellationToken)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            foreach (var user in users)
            {
                var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, new List<string> { roleName });
                if (!removeRolesResult.Succeeded)
                {
                    return CreateIdentityError(removeRolesResult.Errors);
                }
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

        private static Expression<Func<User, object>> GetSortProperty(string? sortColumn)
        {
            return sortColumn?.ToLower() switch
            {
                "firstname" => user => user.FirstName,
                "lastname" => user => user.LastName,
                "email" => user => user.Email!,
                _ => user => user.Id
            };
        }
    }
}
