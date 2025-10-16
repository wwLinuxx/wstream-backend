using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UzTube.Entities;

namespace UzTube.DataAccess.Persistence.Configurations;

public class PlaylistPostConfiguration : IEntityTypeConfiguration<PlaylistPost>
{
    public void Configure(EntityTypeBuilder<PlaylistPost> builder)
    {
        builder.HasKey(p => new
        {
            p.PlaylistId,
            p.PostId
        });
    }
}
