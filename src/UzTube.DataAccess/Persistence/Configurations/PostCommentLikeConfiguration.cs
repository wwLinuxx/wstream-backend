using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UzTube.Entities;

namespace UzTube.DataAccess.Persistence.Configurations;

public class PostCommentLikeConfiguration : IEntityTypeConfiguration<PostCommentLike>
{
    public void Configure(EntityTypeBuilder<PostCommentLike> builder)
    {
        builder.HasKey(l => new
        {
            l.CommentId,
            l.UserId
        });
    }
}