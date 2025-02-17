using Domain.Apps;
using Domain.AppUsers;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ApplicationConfiguration : IEntityTypeConfiguration<App>
    {
        public void Configure(EntityTypeBuilder<App> builder)
        {
            builder.ToTable(TableNames.Apps);

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(256);

            builder.HasIndex(a => a.Name)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.Property(a => a.Description)
                .HasMaxLength(450);
            
            builder.Property(a => a.ClientId)
                .HasMaxLength(256);
            
            builder.Property(a => a.ClientSecret)
                .HasMaxLength(256);
            
            builder.Property(a => a.BaseUrl)
                .HasMaxLength(1000);

            builder.Property(a => a.CreatedOn)
                .IsRequired();

            builder.Property(a => a.CreatedBy)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(a => a.LastModifiedBy)
                .HasMaxLength(256);
            
            builder.Property(a => a.DeletedBy)
                .HasMaxLength(256);

            builder.HasIndex(a => a.IsDeleted);

            builder.Property(a => a.IsDeleted)
            .IsRequired();

            builder.Property(a => a.IsActive)
                .IsRequired();

            builder.HasQueryFilter(a => !a.IsDeleted);

            builder.HasMany(a => a.Roles)
                .WithOne(r => r.App)
                .HasForeignKey(r => r.AppId)
                .IsRequired(false);

            builder.HasMany(a => a.Permissions)
                .WithOne(p => p.App)
                .HasForeignKey(p => p.AppId)
                .IsRequired(false);

            builder.HasMany(a => a.Users)
                .WithMany(u => u.Apps)
                .UsingEntity<AppUser>(
                    x => x.HasOne(au => au.User)
                        .WithMany()
                        .HasForeignKey(au => au.UserId)
                        .IsRequired(false),
                    x => x.HasOne(au => au.App)
                        .WithMany()
                        .HasForeignKey(au => au.AppId)
                        .IsRequired(false)
                )
                .ToTable(TableNames.AppUsers);
        }
    }
}
