using UzTube.Enums;

namespace UzTube.Entities;

public class Post
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Duration { get; set; }

    public int Rating { get; set; }

    public string VideoUrl { get; set; }

    public DateTime PostedAt { get; set; } = DateTime.UtcNow;

    public VideoStatus Status { get; set; } = VideoStatus.Loading;

    public bool IsPrivate = false;

    public ICollection<PostView> PostViews { get; set; }

    public ICollection<PostLike> PostLikes { get; set; }

    public ICollection<PostComment> PostComments { get; set; }

    public ICollection<PostCategory> PostCategories { get; set; }

    public ICollection<PlaylistPost> PlaylistPosts { get; set; }

    public ICollection<PostRating> PostRatings { get; set; }
}
