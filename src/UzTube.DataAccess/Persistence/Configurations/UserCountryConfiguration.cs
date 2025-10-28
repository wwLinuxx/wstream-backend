using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UzTube.Core.Entities;

namespace UzTube.DataAccess.Persistence.Configurations;

public class UserCountryConfiguration : IEntityTypeConfiguration<UserCountry>
{
    public void Configure(EntityTypeBuilder<UserCountry> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasIndex(c => c.Code)
            .IsUnique();

        builder.Property(c => c.Code)
            .HasMaxLength(2)
            .HasMaxLength(3)
            .IsUnicode(false);

        builder.Property(c => c.FullName)
            .HasMaxLength(3)
            .HasMaxLength(25)
            .IsUnicode(false);
    }
}