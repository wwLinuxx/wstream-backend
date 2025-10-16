using UzTube.Core.Common;

namespace UzTube.Entities;

public class PostLike : BaseEntity
{
    public Guid PostId { get; set; }
    public Post Post { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public DateTime LikedAt { get; set; } = DateTime.UtcNow;
}