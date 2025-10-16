using UzTube.Core.Common;

namespace UzTube.Entities;

public class PostCommentLike : BaseEntity
{
    public Guid CommentId { get; set; }
    public PostComment PostComment { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public DateTime LikedAt { get; set; } = DateTime.UtcNow;
}
