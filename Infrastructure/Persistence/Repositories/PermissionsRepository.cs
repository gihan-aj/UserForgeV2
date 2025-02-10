using Application.Abstractions.Repositories;
using Domain.Permissions;
using Domain.Roles;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using SharedKernal;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence.Repositories
{
    public class PermissionsRepository : IPermissionsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<Role> _roleManager;

        public PermissionsRepository(ApplicationDbContext context, RoleManager<Role> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<Result<List<Permission>>> GetAllPermissionsWithAssignedRoles(CancellationToken cancellationToken)
        {
            return await _context.Permissions
                .Include(p => p.RolePermissions)
                .ThenInclude(rp => rp.Role)
                .ToListAsync(cancellationToken);
        }

        public async Task<Result> AssignRolePermissionsAsync(
            Role role, 
            List<string>permissionIds, 
            string modifiedBy, 
            CancellationToken cancellationToken)
        {
            _context.RolePermissions.RemoveRange(role.RolePermissions);

            if (permissionIds.Any())
            {
                var permissions = await _context.Permissions
                    .Where(p => permissionIds.Contains(p.Id))
                    .ToListAsync(cancellationToken);

                if(permissions.Count != permissionIds.Count)
                {
                    return PermissionErrors.NotFound.MissingPermissions;
                }

                role.AddRolePermissionsRange(permissions, modifiedBy);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();

        }

        private static Expression<Func<Permission, object>> GetSortProperty(string? sortColumn)
        {
            return sortColumn?.ToLower() switch
            {
                "name" => permission => permission.Name!,
                "id" => permission => permission.Id,
                _ => permission => permission.Id
            };
        }
    }
}
