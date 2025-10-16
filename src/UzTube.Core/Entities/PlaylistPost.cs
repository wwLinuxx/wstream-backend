using UzTube.Core.Common;

namespace UzTube.Entities;

public class PlaylistPost : BaseEntity
{
    public Guid PlaylistId { get; set; }
    public UserPlaylist UserPlaylist { get; set; }

    public Guid PostId { get; set; }
    public Post Post { get; set; }
}
