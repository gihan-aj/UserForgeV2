using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Domain.Permissions;
using Domain.RolePermissions;
using Domain.RefreshTokens;
using Domain.UserSettings;

namespace Application.Abstractions.Data
{
    public interface IApplicationDbContext
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<UserSetting> UserSettings { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
