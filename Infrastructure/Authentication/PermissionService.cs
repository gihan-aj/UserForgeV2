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

        public Task<HashSet<string>> GetPermissionsAsync(string userId)
        {
            throw new NotImplementedException();
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
