using Application.Abstractions.Services;
using Application.Shared.Pagination;
using Application.UserManagement.Queries.GetAll;
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

            if (users.Count == 0)
            {
                return ids.Count == 1
                    ? Result.Failure<List<string>>(UserErrors.NotFound.User(ids[0]))
                    : Result.Failure<List<string>>(UserErrors.NotFound.Users);
            }

            var activatedUsers = new List<string>();

            foreach (var user in users)
            {
                if (IsUserProtected(user))
                {
                    return Result.Failure<List<string>>(UserErrors.Authorization.ProtectedUser(user.Email!));
                }

                if (!user.IsActive)
                {
                    user.Activate(modifiedBy);
                    var updateResult = await _userManager.UpdateAsync(user);
                    if (!updateResult.Succeeded)
                    {
                        return CreateIdentityError<List<string>>(updateResult.Errors);
                    }

                    activatedUsers.Add(user.Email!);
                }
            }

            if (activatedUsers.Count == 0)
            {
                return ids.Count == 1
                    ? Result.Failure<List<string>>(UserErrors.General.OperationFailed("The user is already activated."))
                    : Result.Failure<List<string>>(UserErrors.General.OperationFailed("Users are already activated."));
            }

            return activatedUsers;
        }
        
        public async Task<Result<List<string>>> DeactivateUsersAsync(List<string> ids, string modifiedBy, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users
                .Where(u => ids.Contains(u.Id))
                .ToListAsync(cancellationToken);

            if (users.Count == 0)
            {
                return ids.Count == 1
                    ? Result.Failure<List<string>>(UserErrors.NotFound.User(ids[0]))
                    : Result.Failure<List<string>>(UserErrors.NotFound.Users);
            }

            var deactivatedUsers = new List<string>();

            foreach (var user in users)
            {
                if (IsUserProtected(user))
                {
                    return Result.Failure<List<string>>(UserErrors.Authorization.ProtectedUser(user.Email!));
                }

                if (user.IsActive)
                {
                    user.Deactivate(modifiedBy);
                    var updateResult = await _userManager.UpdateAsync(user);
                    if (!updateResult.Succeeded)
                    {
                        return CreateIdentityError<List<string>>(updateResult.Errors);
                    }

                    deactivatedUsers.Add(user.Email!);
                }            
            }

            if (deactivatedUsers.Count == 0)
            {
                return ids.Count == 1
                    ? Result.Failure<List<string>>(UserErrors.General.OperationFailed("The user is already deactivated."))
                    : Result.Failure<List<string>>(UserErrors.General.OperationFailed("Users are already deactivated."));
            }

            return deactivatedUsers;
        }

        public async Task<Result<string[]>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return Result.Failure<string[]>(UserErrors.NotFound.User(userId));
            }
            IList<string> roles = await _userManager.GetRolesAsync(user);
            return roles.ToArray();
        }

        public async Task<Result<List<string>>> AssignUserRolesAsync(string userId, List<string> roleNames, string modifiedBy, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return Result.Failure<List<string>>(UserErrors.NotFound.User(userId));
            }

            if (IsUserProtected(user))
            {
                return Result.Failure<List<string>>(UserErrors.Authorization.ProtectedUser(user.Email!));
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

            user.UserRolesChanged(modifiedBy);
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

            if(users.Count == 0)
            {
                return ids.Count == 1
                    ? Result.Failure<List<string>>(UserErrors.NotFound.User(ids[0]))
                    : Result.Failure<List<string>>(UserErrors.NotFound.Users);
            }

            var deletedUsers = new List<string>();

            foreach (var user in users)
            {
                if (IsUserProtected(user))
                {
                    return Result.Failure<List<string>>(UserErrors.Authorization.ProtectedUser(user.Email!));
                }

                user.DeletedBy = deletedBy;
                var deletedResult = await _userManager.DeleteAsync(user);
                if (!deletedResult.Succeeded)
                {
                    return CreateIdentityError<List<string>>(deletedResult.Errors);
                }

                var deletedUserRoles = await _userManager.GetRolesAsync(user);
                if(deletedUserRoles.Count > 0)
                {
                    var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, deletedUserRoles);
                    if (!removeRolesResult.Succeeded)
                    {
                        return CreateIdentityError<List<string>>(removeRolesResult.Errors);
                    }
                }

                deletedUsers.Add(user.Email!);
            }

            if(deletedUsers.Count == 0)
            {
                return ids.Count == 1
                    ? Result.Failure<List<string>>(UserErrors.General.OperationFailed("The user is already deleted."))
                    : Result.Failure<List<string>>(UserErrors.General.OperationFailed("Users are already deleted."));
            }

            return deletedUsers;
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

        private bool IsUserProtected(User user)
        {
            return ProtectedUsers.Emails.Contains(user.Email!);
        }
    }
}
