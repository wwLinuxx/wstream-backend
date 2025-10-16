using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UzTube.Entities;

namespace UzTube.DataAccess.Persistence.Configurations;

public class UserCountryConfiguration : IEntityTypeConfiguration<UserCountry>
{
    public void Configure(EntityTypeBuilder<UserCountry> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasIndex(c => c.Id)
            .IsUnique(false);

        builder.Property(c => c.Id)
            .HasMaxLength(50)
            .IsUnicode(false);
    }
}
