using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class Comment : BaseEntity
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public string CommentText { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Post Post { get; set; } = null!;
    public User User { get; set; } = null!;
    public ICollection<CommentLike> CommentLikes { get; set; } = [];
}