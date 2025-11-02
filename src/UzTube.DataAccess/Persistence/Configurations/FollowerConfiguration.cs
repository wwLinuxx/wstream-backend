using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UzTube.Core.Entities;

namespace UzTube.DataAccess.Persistence.Configurations;

public class FollowerConfiguration : IEntityTypeConfiguration<Follower>
{
    public void Configure(EntityTypeBuilder<Follower> builder)
    {
        builder.HasKey(uf => new
        {
            uf.FollowingId,
            uf.FollowerId
        });

        builder.HasOne(uf => uf.Follow)
            .WithMany(f => f.Followers)
            .HasForeignKey(uf => uf.FollowerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(uf => uf.Following)
            .WithMany(f => f.Followings)
            .HasForeignKey(uf => uf.FollowingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
