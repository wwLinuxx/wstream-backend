using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UzTube.Core.Entities;

namespace UzTube.DataAccess.Persistence.Configurations;

public class StreamConfiguration : IEntityTypeConfiguration<Core.Entities.Stream>
{
    public void Configure(EntityTypeBuilder<Core.Entities.Stream> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.StreamKey)
            .IsRequired()
            .HasMaxLength(16)
            .IsUnicode(false);
        builder.HasIndex(s => s.StreamKey)
            .IsUnique();

        builder.Property(s => s.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(s => s.User)
            .WithMany(u => u.LiveStreams)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
