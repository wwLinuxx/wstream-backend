using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UzTube.Entities;

namespace UzTube.DataAccess.Persistence.Configurations;

public class PostViewConfiguration : IEntityTypeConfiguration<PostView>
{
    public void Configure(EntityTypeBuilder<PostView> builder)
    {
        builder.HasKey(pv => new
        {
            pv.Id,
            pv.UserId,
            pv.PostId
        });

        builder.HasOne(pv => pv.User)
            .WithMany(u => u.PostViews)
            .HasForeignKey(pv => pv.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pv => pv.Post)
            .WithMany(p => p.PostViews)
            .HasForeignKey(pv => pv.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
