using Domain.Common;
using Domain.RefreshTokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    internal class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Token).HasMaxLength(450).IsRequired();
            builder.Property(rt => rt.DeviceIdentifierHash).HasMaxLength(450).IsRequired();

            builder.HasIndex(rt => rt.UserId);

            builder.HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .IsRequired(false);

            builder.ToTable(TableNames.RefreshTokens);
        }
    }
}
