using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UzTube.Core.Common;
using UzTube.Core.Entities;
using UzTube.Shared.Helpers.Interfaces;

namespace UzTube.DataAccess.Persistence.Configurations;

public class UserConfiguration(
    IPasswordHasher passwordHasher
) : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);
        builder.HasIndex(u => u.Username)
            .IsUnique();

        builder.Property(u => u.Email)
             .IsRequired()
             .HasMaxLength(100)
             .IsUnicode(false);
        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(128);

        builder.HasOne(u => u.Country)
            .WithMany(c => c.Users)
            .HasForeignKey(u => u.CountryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData(GetSeedUser());
    }

    private User GetSeedUser() => new()
    {
        Id = SystemIds.User.Root,
        Username = "wstream",
        Email = "wstream@wstream.uz",
        PasswordHash = passwordHasher.Hash(SystemPasswords.User.Root),
        CreatedOn = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    };
}