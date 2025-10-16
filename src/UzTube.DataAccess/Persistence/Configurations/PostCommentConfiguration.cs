using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UzTube.Entities;

namespace UzTube.DataAccess.Persistence.Configurations;

public class PostCommentConfiguration : IEntityTypeConfiguration<PostComment>
{
    public void Configure(EntityTypeBuilder<PostComment> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasOne(c => c.Post)
            .WithMany(p => p.PostComments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.User)
            .WithMany(u => u.PostComments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(c => c.Comment)
            .HasMaxLength(1500)
            .IsUnicode(false);
    }
}
