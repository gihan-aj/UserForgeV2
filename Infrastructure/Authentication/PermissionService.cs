using Application.Abstractions.Data;
using Application.Abstractions.Services;
using Domain.Roles;
using Domain.Users;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Authentication
{
    public class PermissionService : IPermissionService
    {
        private readonly ApplicationDbContext _context;

        public PermissionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HashSet<string>> GetPermissionsAsync(string userId)
        {
            IEnumerable<Role>[] roles = await _context.Set<User>()
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .Where(u => u.Id == userId)
                .Select(u => u.UserRoles.Select(ur => ur.Role))
                .ToArrayAsync();

            return roles
                .SelectMany(r => r)
                .SelectMany(u => u.RolePermissions)
                .Select(ur => ur.Permission)
                .Select(p => p.Name)
                .ToHashSet();
        }
        
        public async Task<HashSet<string>> GetPermissionsAsync(string userId, CancellationToken cancellationToken)
        {
            IEnumerable<Role>[] roles = await _context.Set<User>()
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .Where(u => u.Id == userId)
                .Select(u => u.UserRoles.Select(ur => ur.Role))
                .ToArrayAsync(cancellationToken);

            return roles
                .SelectMany(r => r)
                .SelectMany(u => u.RolePermissions)
                .Select(ur => ur.Permission)
                .Select(p => p.Name)
                .ToHashSet();
        }

        //public async Task<HashSet<string>> GetPermissionsAsync(string userId)
        //{
        //    ICollection<Role>[] roles = await _context.Set<User>()
        //        .Include(x => x.Roles)
        //        .ThenInclude(x => x.Permissions)
        //        .Where(x => x.Id == userId)
        //        .Select(x => x.Roles)
        //        .ToArrayAsync();

        //    return roles
        //        .SelectMany(x => x)
        //        .SelectMany(x => x.Permissions)
        //        .Select(x => x.Name)
        //        .ToHashSet();
        //}
    }
}
