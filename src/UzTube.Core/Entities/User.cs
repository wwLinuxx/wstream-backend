using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; } = false;
    public DateTime? UpdatedOn { get; set; }
    public DateTime? DeletedOn { get; set; }

    public UserProfile Profile { get; set; } = null!;
    public ICollection<UserFollower> Followers { get; set; } = [];
    public ICollection<UserFollower> Followings { get; set; } = [];
    public ICollection<Post> Posts { get; set; } = [];
    public ICollection<PostView> Views { get; set; } = [];
    public ICollection<PostLike> Likes { get; set; } = [];
    public ICollection<PostComment> Comments { get; set; } = [];
    public ICollection<PostCommentLike> CommentLikes { get; set; } = [];
    public ICollection<UserPlaylist> Playlists { get; set; } = [];
    public ICollection<PostRating> Ratings { get; set; } = [];
    public ICollection<UserRole> Roles { get; set; } = [];
}