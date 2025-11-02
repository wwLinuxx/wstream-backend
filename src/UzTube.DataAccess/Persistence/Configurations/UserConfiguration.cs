using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UzTube.Core.Common;
using UzTube.Core.Entities;
using User = UzTube.Core.Entities.User;

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

        builder.HasData(GetSeedUser());
    }

    private static string Encrypt(string password, string salt)
    {
        using Rfc2898DeriveBytes algorithm = new Rfc2898DeriveBytes(
            password,
            Encoding.UTF8.GetBytes(salt),
            8192,
            HashAlgorithmName.SHA256);

        return Convert.ToBase64String(algorithm.GetBytes(50));
    }

    private static User GetSeedUser()
    {
        Guid seedRootId = SystemIds.User.Root;
        string seedRootPassword = SystemPasswords.User.Root;
        string seedRootSalt = SystemIds.Salt.Root;

        return new User
        {
            Id = seedRootId,
            Email = "uztube@uztube.uz",
            PasswordHash = Encrypt(seedRootPassword, seedRootSalt),
            Salt = seedRootSalt,
            CreatedOn = DateTime.UtcNow
        };;
    }
}