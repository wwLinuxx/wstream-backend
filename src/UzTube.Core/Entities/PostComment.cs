namespace UzTube.Entities;

public class PostComment
{
    public int Id { get; set; }

    public int PostId { get; set; }
    public Post Post { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public string Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<PostCommentLike> PostCommentLikes { get; set; }
}
