using Domain.Common;
using Domain.Users;
using Domain.UserSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    internal sealed class UserSettingConfiguration : IEntityTypeConfiguration<UserSetting>
    {
        public void Configure(EntityTypeBuilder<UserSetting> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(us => us.Key).IsRequired().HasMaxLength(50);
            builder.Property(us => us.Value).IsRequired();
            builder.Property(us => us.DataType).HasMaxLength(20);

            builder.HasIndex(us => us.UserId);

            builder.HasOne<User>()
                .WithMany(u => u.UserSettings)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable(TableNames.UserSettings);
        }
    }
}
