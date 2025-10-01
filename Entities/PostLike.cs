namespace UzTube.Entities;

public class PostLike
{
    public int PostId { get; set; }
    public Post Post { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public DateTime LikedAt { get; set; } = DateTime.UtcNow;
}