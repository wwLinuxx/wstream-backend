using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class Like : BaseEntity
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public DateTime LikedAt { get; set; } = DateTime.UtcNow;

    public Post Post { get; set; } = null!;
    public User User { get; set; } = null!;
}