namespace UzTube.Entities;

public class PostCommentLike
{
    public int CommentId { get; set; }
    public PostComment PostComment { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public DateTime LikedAt { get; set; } = DateTime.UtcNow;
}
