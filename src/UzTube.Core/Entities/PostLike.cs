using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class PostLike : BaseEntity
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public DateTime LikedAt { get; set; } = DateTime.Now;

    public Post Post { get; set; } = null!;
    public User User { get; set; } = null!;
}