using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Domain.Roles;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class RolesRepository: IRolesRepository
    {
        private readonly IApplicationDbContext _context;

        public RolesRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Role role)
        {
            _context.Roles.Add(role);
        }

        public string NormalizeRoleName(string roleName)
        {
            return _context.NormalizeKey(roleName);
        }

        public async Task<bool> RoleNameExists(string roleName, int appId)
        {
            return await _context.Roles.AnyAsync(r => r.Name == roleName && r.AppId == appId);
        }
    }
}
