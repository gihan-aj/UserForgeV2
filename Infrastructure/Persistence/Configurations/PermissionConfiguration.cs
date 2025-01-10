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

            builder.HasIndex(x => x.Name).IsUnique().HasFilter("[IsDeleted] = 0");
            builder.HasIndex(x => x.IsDeleted);

            builder.HasQueryFilter(u => !u.IsDeleted);

            // Maps to the AspNetRoles table
            builder.ToTable("Permissions");

        }
    }
}
