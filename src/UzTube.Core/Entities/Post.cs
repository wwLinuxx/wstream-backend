using UzTube.Core.Common;
using UzTube.Enums;

namespace UzTube.Core.Entities;

public class Post : BaseEntity
{
    // TODO: Need change SoftDelete to HistoryTable
    public Guid UserId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string PreviewUrl { get; set; } = null!;
    public string VideoUrl { get; set; } = null!;
    public string Duration { get; set; } = null!;
    public int ViewsCount { get; set; }
    public int LikesCount { get; set; }
    public int Rating { get; set; }
    public DateTime PostedOn { get; set; } = DateTime.UtcNow;
    public VideoStatus Status { get; set; } = VideoStatus.Checking;
    public bool IsPrivate { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public User User { get; set; } = null!;
    public ICollection<View> Views { get; set; } = [];
    public ICollection<Like> Likes { get; set; } = [];
    public ICollection<Comment> Comments { get; set; } = [];
    public ICollection<PostCategory> Categories { get; set; } = [];
    public ICollection<PlaylistPost> PlaylistPosts { get; set; } = [];
    public ICollection<Rating> Ratings { get; set; } = [];
}