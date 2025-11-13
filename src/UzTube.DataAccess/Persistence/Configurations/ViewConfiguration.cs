using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UzTube.Core.Entities;

namespace UzTube.DataAccess.Persistence.Configurations;

public class ViewConfiguration : IEntityTypeConfiguration<View>
{
    public void Configure(EntityTypeBuilder<View> builder)
    {
        // TODO: Need refactor HasKey
        builder.HasKey(pv => new
        {
            pv.Id,
            pv.UserId,
            pv.PostId
        });

        builder.HasOne(pv => pv.User)
            .WithMany(u => u.Views)
            .HasForeignKey(pv => pv.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pv => pv.Post)
            .WithMany(p => p.Views)
            .HasForeignKey(pv => pv.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}