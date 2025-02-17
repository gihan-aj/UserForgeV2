using Domain.Common;
using Domain.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    internal class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // Primary key
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Description).HasMaxLength(450);

            // Index for "normalized" role name to allow efficient lookups
            builder.HasIndex(r => new { r.NormalizedName , r.AppId })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.HasIndex(r => r.IsDeleted);

            builder.Property(r => r.CreatedBy)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(r => r.LastModifiedBy)
                .HasMaxLength(256);
            
            builder.Property(r => r.PermissionModifiedBy)
                .HasMaxLength(256);

            builder.Property(r => r.DeletedBy)
                .HasMaxLength(256);

            builder.HasQueryFilter(r => !r.IsDeleted);

            // A concurrency token for use with the optimistic concurrency checking
            builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

            // Limit the size of columns to use efficient database types
            builder.Property(r => r.Name).HasMaxLength(256);
            builder.Property(r => r.NormalizedName).HasMaxLength(256);

            // The relationships between Role and other entity types
            // Note that these relationships are configured with no navigation properties

            // Each Role can have many entries in the UserRole join table
            builder.HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            // Each Role can have many associated RoleClaims
            builder.HasMany<IdentityRoleClaim<string>>()
                .WithOne()
                .HasForeignKey(rc => rc.RoleId)
                .IsRequired();

            // Each Role can have many associated RolePermissions
            builder.HasMany(r => r.RolePermissions)
                .WithOne(rp => rp.Role)
                .HasForeignKey(rp => rp.RoleId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            // Maps to the AspNetRoles table
            builder.ToTable(TableNames.Roles);
        }
    }
}
