using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Infrastructure.Persistence.Configurations
{
    internal class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Token).HasMaxLength(450);
            builder.Property(rt => rt.DeviceInfo).HasMaxLength(450);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .IsRequired();

            builder.ToTable("RefreshTokens");
        }
    }
}
