using Domain.Common;
using Domain.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
            builder.Property(x => x.Description).HasMaxLength(450);
            builder.Property(x => x.Order).IsRequired();

            builder.HasIndex(x => new { x.Name, x.AppId }).IsUnique();
            builder.HasIndex(x => x.Order).IsUnique();

            builder.HasMany(p => p.RolePermissions)
                .WithOne(rp => rp.Permission)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.ToTable(TableNames.Permissions);
        }
    }
}
