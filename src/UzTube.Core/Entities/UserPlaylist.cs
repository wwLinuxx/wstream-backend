using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class UserPlaylist : BaseEntity
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public bool IsPrivate { get; set; } = true;

    public User User { get; set; } = null!;

    public ICollection<PlaylistPost> PlaylistPosts { get; set; } = [];
}