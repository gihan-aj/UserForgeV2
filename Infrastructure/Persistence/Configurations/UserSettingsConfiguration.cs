using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    internal class UserSettingsConfiguration : IEntityTypeConfiguration<UserSettings>
    {
        public void Configure(EntityTypeBuilder<UserSettings> builder)
        {
            builder.HasKey(us => us.Id);

            builder.Property(us => us.Theme).HasMaxLength(50).HasDefaultValue("light");
            builder.Property(us => us.Language).HasMaxLength(10).HasDefaultValue("en");
            builder.Property(us => us.DateFormat).HasMaxLength(20).HasDefaultValue("MM/dd/yyyy");
            builder.Property(us => us.TimeFormat).HasMaxLength(20).HasDefaultValue("hh:mm tt");
            builder.Property(us => us.TimeZone).HasMaxLength(100).HasDefaultValue("UTC");

            builder.Property(us => us.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne<User>().WithOne().HasForeignKey<UserSettings>(us => us.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("UserSettings");
        }
    }
}
