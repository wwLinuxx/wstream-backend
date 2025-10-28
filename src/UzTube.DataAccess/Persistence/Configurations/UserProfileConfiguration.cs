using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UzTube.Core.Entities;

namespace UzTube.DataAccess.Persistence.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(up => up.UserId);

        builder.HasOne(up => up.User)
            .WithOne(u => u.Profile)
            .HasForeignKey<UserProfile>(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(up => up.FirstName)
            .HasMaxLength(50)
            .IsUnicode(false);
        builder.Property(up => up.LastName)
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.HasIndex(up => up.PhoneNumber)
            .IsUnique();

        builder.ToTable(tb =>
            tb.HasCheckConstraint("CK_UserProfile_Age_Min_7_Max_90", "\"Age\" BETWEEN 7 AND 90"));

        builder.HasOne(up => up.Country)
            .WithOne(c => c.UserProfile)
            .HasForeignKey<UserProfile>(up => up.CountryId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
