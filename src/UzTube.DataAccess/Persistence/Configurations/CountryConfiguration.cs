using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UzTube.Core.Common;
using UzTube.Core.Entities;

namespace UzTube.DataAccess.Persistence.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
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

        builder.HasData(new Country
        {
            Id = SystemIds.Country.Uzbekistan,
            Code = "UZ",
            FullName = "Uzbekistan"
        });
    }
}