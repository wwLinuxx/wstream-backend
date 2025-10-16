using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UzTube.Entities;

namespace UzTube.DataAccess.Persistence.Configurations;

public class PostRatingConfiguration : IEntityTypeConfiguration<PostRating>
{
    public void Configure(EntityTypeBuilder<PostRating> builder)
    {
        builder.HasKey(r => new
        {
            r.Id,
            r.PostId,
            r.UserId
        });

        builder.HasOne(r => r.Post)
            .WithMany(p => p.PostRatings)
            .HasForeignKey(r => r.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.User)
            .WithMany(u => u.PostRatings)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
