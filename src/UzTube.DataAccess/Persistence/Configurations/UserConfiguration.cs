using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UzTube.Core.Common;
using UzTube.Core.Entities;

namespace UzTube.DataAccess.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        //entity.HasIndex(u => u.Username)
        //    .IsUnique();

        //entity.Property(u => u.Username)
        //    .HasMaxLength(50)
        //    .IsUnicode(false);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.Email)
            .HasMaxLength(100)
            .IsUnicode(false);
    }

    private static string Encrypt(string password, string salt)
    {
        using var algorithm = new Rfc2898DeriveBytes(
            password,
            Encoding.UTF8.GetBytes(salt),
            1000,
            HashAlgorithmName.SHA256);

        return Convert.ToBase64String(algorithm.GetBytes(32));
    }

    private static User GetSeedUser()
    {
        var seedRootId = SystemIds.User.Root;
        var seedRootPassword = SystemIds.Password.Root;
        var seedRootSalt = SystemIds.Salt.Root;

        var seedRootUser = new User
        {
            Id = seedRootId,
            Email = "wwstream@wwstream.uz",
            PasswordHash = Encrypt(seedRootPassword, seedRootSalt),
            Salt = seedRootSalt,
            CreatedOn = DateTime.Now
        };

        return seedRootUser;
    }
}