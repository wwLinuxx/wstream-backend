using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class Playlist : BaseEntity
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsPrivate { get; set; } = true;

    public User User { get; set; } = null!;

    public ICollection<PlaylistPost> PlaylistPosts { get; set; } = [];
}