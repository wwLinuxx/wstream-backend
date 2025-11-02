using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UzTube.Core.Entities;

namespace UzTube.DataAccess.Persistence.Configurations;

public class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
{
    public void Configure(EntityTypeBuilder<Playlist> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasOne(up => up.User)
            .WithMany(u => u.Playlists)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(up => up.Name)
            .HasMaxLength(50)
            .IsUnicode(false);
    }
}
