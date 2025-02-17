using Domain.Primitives;
using Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using Application.Abstractions.Data;
using Domain.Roles;
using Domain.Permissions;
using Domain.RolePermissions;
using Domain.RefreshTokens;
using Microsoft.AspNetCore.Identity;
using Domain.UserRoles;
using Domain.UserSettings;
using Domain.Apps;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext 
        : IdentityDbContext<
            User, 
            Role, 
            string,
            IdentityUserClaim<string>,
            UserRole,
            IdentityUserLogin<string>,
            IdentityRoleClaim<string>,
            IdentityUserToken<string>>
        , Application.Abstractions.Data.IApplicationDbContext
        , IUnitOfWork
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<UserSetting> UserSettings { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<App> Apps { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            var softDeleteEntries = ChangeTracker
                .Entries<ISoftDeletable>()
                .Where(e => e.State == EntityState.Deleted);

            foreach (var entry in softDeleteEntries)
            {
                entry.State = EntityState.Modified;
                entry.Property(nameof(ISoftDeletable.IsDeleted)).CurrentValue = true;
                entry.Property(nameof(ISoftDeletable.DeletedOn)).CurrentValue = DateTime.UtcNow;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
