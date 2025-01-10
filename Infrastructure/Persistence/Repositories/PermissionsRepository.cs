using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Domain.Permissions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class PermissionsRepository : IPermissionsRepository
    {
        private readonly IApplicationDbContext _context;

        public PermissionsRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Permission permission)
        {
            _context.Permissions.Add(permission);
        }

        public void Remove(Permission permission)
        {
            _context.Permissions.Remove(permission);
        }

        public void Update(Permission permission)
        {
            _context.Permissions.Update(permission);
        }

        public async Task<bool> NameExistsAsync(string permissionName)
        {
            return await _context.Permissions.AnyAsync(p => p.Name == permissionName);
        }

        public async Task<Permission?> GetByIdAsync(string id)
        {
            return await _context.Permissions.FindAsync(id);
        }
    }
}
