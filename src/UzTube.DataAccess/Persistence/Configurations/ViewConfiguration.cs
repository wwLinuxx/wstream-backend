using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UzTube.Core.Entities;

namespace UzTube.DataAccess.Persistence.Configurations;

public class ViewConfiguration : IEntityTypeConfiguration<View>
{
    public void Configure(EntityTypeBuilder<View> builder)
    {
        builder.HasKey(pv => pv.Id);

        builder.HasIndex(pv => new { pv.UserId, pv.PostId })
            .IsUnique()
            .HasFilter("\"UserId\" IS NOT NULL");

        builder.HasOne(pv => pv.User)
            .WithMany(u => u.Views)
            .HasForeignKey(pv => pv.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pv => pv.Post)
            .WithMany(p => p.Views)
            .HasForeignKey(pv => pv.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}