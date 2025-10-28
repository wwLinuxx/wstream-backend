using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class PostCommentLike : BaseEntity
{
    public Guid CommentId { get; set; }
    public Guid UserId { get; set; }
    public DateTime LikedAt { get; set; } = DateTime.Now;

    public PostComment PostComment { get; set; } = null!;
    public User User { get; set; } = null!;
}