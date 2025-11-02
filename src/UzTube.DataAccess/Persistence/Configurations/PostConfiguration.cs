using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UzTube.Core.Entities;

namespace UzTube.DataAccess.Persistence.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(p => p.Title)
            .HasMaxLength(100)
            .IsUnicode(false);

        builder.Property(p => p.Description)
            .HasMaxLength(1000)
            .IsUnicode(false);

        builder.Property(p => p.Duration)
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Property(p => p.ThumbnailUrl)
            .HasMaxLength(1000)
            .IsUnicode(false);

        builder.Property(p => p.VideoUrl)
            .HasMaxLength(1000)
            .IsUnicode(false);
    }
}