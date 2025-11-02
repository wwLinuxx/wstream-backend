using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class CommentLike : BaseEntity
{
    public Guid CommentId { get; set; }
    public Guid UserId { get; set; }
    public DateTime LikedAt { get; set; } = DateTime.UtcNow;

    public Comment PostComment { get; set; } = null!;
    public User User { get; set; } = null!;
}