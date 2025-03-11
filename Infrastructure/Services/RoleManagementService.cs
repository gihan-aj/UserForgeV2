using Application.Abstractions.Services;
using Domain.Roles;
using Microsoft.AspNetCore.Identity;
using SharedKernal;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using System.Linq;
using Application.Shared.Pagination;
using System.Threading;
using Application.Roles.Queries.GetAll;
using Microsoft.EntityFrameworkCore;
using Application.Abstractions.Data;

namespace Infrastructure.Services
{
    public class RoleManagementService : IRoleManagementService
    {
        private readonly RoleManager<Role> _roleManager;
        public RoleManagementService(RoleManager<Role> roleManager, IApplicationDbContext context)
        {
            _roleManager = roleManager;
        }

        public async Task<Result<string>> CreateAsync(string roleName, string description, int appId, string userId)
        {
            if(await _roleManager.Roles.AnyAsync(r => r.Name == roleName && r.AppId == appId))
            {
                return Result.Failure<string>(RoleErrors.Conflict.RoleNameAlreadyExists(roleName));
            }

            //var role = new IdentityRole(roleName);
            var role = new Role(roleName, description, appId, userId);
            var normalizedName = _roleManager.NormalizeKey(roleName);
            role.NormalizedName = normalizedName;

            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                return CreateIdentityError<string>(result.Errors);
            }

            return role.Id;
        }

        public async Task<Result> UpdateAsync(string roleId, string roleName, string description, string userId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if(role is null)
            {
                return RoleErrors.NotFound.Role(roleId);
            }

            if (IsProtectedRole(role))
            {
                return RoleErrors.Authorization.ProtectedRole(role.Name!);
            }

            var normalizedRoleName = _roleManager.NormalizeKey(roleName);
            role.Update(roleName, normalizedRoleName, description, userId);

            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                return CreateIdentityError(result.Errors);
            }

            return Result.Success();
        }

        public async Task<PaginatedList<GetAllRolesResponse>> GetAllRolesAsync(
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            int page,
            int pageSize,
            int appId,
            CancellationToken cancellationToken)
        {
            IQueryable<Role> rolesQuery = _roleManager.Roles
                .Where(r => r.AppId == appId)
                .AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                rolesQuery = rolesQuery
                    .Where(r => r.Name!.Contains(searchTerm));
            }

            // Sorting
            if (sortOrder?.Trim().ToLower() == "desc")
            {
                rolesQuery = rolesQuery
                    .OrderByDescending(GetSortProperty(sortColumn));
            }
            else
            {
                rolesQuery = rolesQuery
                    .OrderBy(GetSortProperty(sortColumn));
            }

            // Selecting
            var rolesResponsQuery = rolesQuery
                .Select(r => new GetAllRolesResponse(
                    r.Id,
                    r.Name!,
                    r.Description,
                    r.IsActive));

            var roles = await PaginatedList<GetAllRolesResponse>.CreateAsync(
                rolesResponsQuery,
                page,
                pageSize,
                cancellationToken);

            return roles;
        }

        public async Task<string[]> GetRoleNamesAsync(int appId, CancellationToken cancellationToken)
        {
            string[] roleNames = await _roleManager.Roles
                .Where(r => r.AppId == appId)
                .Select(rn => rn.Name!)
                .ToArrayAsync(cancellationToken);

            return roleNames;
        }

        public async Task<Role?> GetRoleById(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }

        public async Task<Role?> GetRoleWithRolePermissionsAsync(string id)
        {
            return await _roleManager.Roles
                .Include(r => r.App)
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Result<List<string>>> ActivateRolesAsync(List<string> ids, string modifiedBy, CancellationToken cancellationToken)
        {
            var roles = await _roleManager.Roles
                .Where(r => ids.Contains(r.Id))
                .ToListAsync(cancellationToken);

            if(roles.Count() == 0)
            {
                return ids.Count == 1
                    ? Result.Failure<List<string>>(RoleErrors.NotFound.Role(ids[0]))
                    : Result.Failure<List<string>>(RoleErrors.NotFound.Roles);
            }

            var activatedRoles = new List<string>();

            foreach (var role in roles)
            {
                if (IsProtectedRole(role))
                {
                    return Result.Failure<List<string>>(RoleErrors.Authorization.ProtectedRole(role.Name!));
                }

                if (!role.IsActive)
                {
                    role.Activate(modifiedBy);
                    var result = await _roleManager.UpdateAsync(role);
                    if (!result.Succeeded)
                    {
                        return CreateIdentityError<List<string>>(result.Errors);
                    }

                    activatedRoles.Add(role.Name!);
                }

            }

            return activatedRoles;
        }

        public async Task<Result<List<string>>> DeactivateRolesAsync(List<string> ids, string modifiedBy, CancellationToken cancellationToken)
        {
            var roles = await _roleManager.Roles
                .Where(r => ids.Contains(r.Id))
                .ToListAsync(cancellationToken);

            if (roles.Count == 0)
            {
                return ids.Count == 1
                    ? Result.Failure<List<string>>(RoleErrors.NotFound.Role(ids[0]))
                    : Result.Failure<List<string>>(RoleErrors.NotFound.Roles);
            }

            var deactivatedRoles = new List<string>();

            foreach (var role in roles)
            {
                if(IsProtectedRole(role))
                {
                    return Result.Failure<List<string>>(RoleErrors.Authorization.ProtectedRole(role.Name!));
                }

                if (role.IsActive)
                {
                    role.Deactivate(modifiedBy);
                    var result = await _roleManager.UpdateAsync(role);
                    if (!result.Succeeded)
                    {
                        return CreateIdentityError<List<string>>(result.Errors);
                    }
                    deactivatedRoles.Add(role.Name!);
                }
            }

            return deactivatedRoles;
        }
        
        public async Task<Result<List<string>>> DeleteRolesAsync(List<string> ids, string deletedBy, CancellationToken cancellationToken)
        {
            var roles = await _roleManager.Roles
                .Where(r => ids.Contains(r.Id))
                .ToListAsync(cancellationToken);

            if (roles.Count == 0)
            {
                return ids.Count == 1
                    ? Result.Failure<List<string>>(RoleErrors.NotFound.Role(ids[0]))
                    : Result.Failure<List<string>>(RoleErrors.NotFound.Roles);
            }

            var deletedNames = new List<string>();

            foreach (var role in roles)
            {
                if (IsProtectedRole(role))
                {
                    return Result.Failure<List<string>>(RoleErrors.Authorization.ProtectedRole(role.Name!));
                }

                if (!role.IsDeleted)
                {
                    role.DeletedBy = deletedBy;

                    var result = await _roleManager.DeleteAsync(role);
                    if (!result.Succeeded)
                    {
                        return CreateIdentityError<List<string>>(result.Errors);
                    }

                    deletedNames.Add(role.Name!);
                }
            }

            return deletedNames;

        }

        private Result CreateIdentityError(IEnumerable<IdentityError> errors)
        {
            var subErrors = errors
                .Select(identityError => new Error(identityError.Code, identityError.Description))
                .ToList();

            var error = new Error("IdentityError", "A problem occured during operation.", subErrors);

            return Result.Failure(error);
        }

        private Result<T> CreateIdentityError<T>(IEnumerable<IdentityError> errors)
        {
            var subErrors = errors
                .Select(identityError => new Error(identityError.Code, identityError.Description))
                .ToList();

            var error = new Error("IdentityError", "A problem occured during operation.", subErrors);

            return Result.Failure<T>(error);
        }

        private static Expression<Func<Role, object>> GetSortProperty(string? sortColumn)
        {
            return sortColumn?.ToLower() switch
            {
                "isActive" => role => role.IsActive,
                "name" => role => role.Name!,
                "id" => role => role.Id,
                _ => role => role.Id
            };
        }

        private bool IsProtectedRole(Role role)
        {
            return SsoAppDefaultRoleConstants.AllRoles.Contains(role.Name);
        }
    }
}
