using UzTube.Core.Common;

namespace UzTube.Entities;

public class PostComment : BaseEntity
{
    public Guid PostId { get; set; }
    public Post Post { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public string Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<PostCommentLike> PostCommentLikes { get; set; }
}
