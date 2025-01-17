using Domain.Common;
using Domain.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Persistence.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
            builder.Property(x => x.Description).HasMaxLength(450);

            builder.HasIndex(x => x.Name).IsUnique().HasFilter("[IsDeleted] = 0");
            builder.HasIndex(x => x.IsDeleted);

            builder.HasMany(p => p.RolePermissions)
                .WithOne(rp => rp.Permission)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.HasQueryFilter(u => !u.IsDeleted);

            builder.ToTable(TableNames.Permissions);

            //IEnumerable<Permission> permissions = DefaultPermissions.AllPermissions
            //    .Select(p => Permission.Create(p, null, "default"));

            //builder.HasData(permissions);
        }
    }
}
