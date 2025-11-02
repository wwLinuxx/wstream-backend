using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class PlaylistPost : BaseEntity
{
    public Guid PlaylistId { get; set; }
    public Guid PostId { get; set; }

    public Playlist UserPlaylist { get; set; } = null!;
    public Post Post { get; set; } = null!;
}