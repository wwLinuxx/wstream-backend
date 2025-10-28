using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class PostComment : BaseEntity
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public string Comment { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Post Post { get; set; } = null!;
    public User User { get; set; } = null!;

    public ICollection<PostCommentLike> CommentLikes { get; set; } = [];
}