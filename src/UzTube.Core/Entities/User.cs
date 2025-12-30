using UzTube.Core.Common;
using UzTube.Core.Enums;

namespace UzTube.Core.Entities;

public class User : BaseEntity
{
    // TODO: Need delete SaftDelete and change to HistoryTable
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public Guid CountryId { get; set; }
    public UserStatus Status { get; set; } = UserStatus.NotVerified;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public DateTime? UpdatedOn { get; set; }
    public DateTime? DeletedOn { get; set; }

    public Country Country { get; set; } = null!;
    public ICollection<Follower> Followers { get; set; } = [];
    public ICollection<Follower> Followings { get; set; } = [];
    public ICollection<Post> Posts { get; set; } = [];
    public ICollection<View> Views { get; set; } = [];
    public ICollection<Like> Likes { get; set; } = [];
    public ICollection<Comment> Comments { get; set; } = [];
    public ICollection<CommentLike> CommentLikes { get; set; } = [];
    public ICollection<Playlist> Playlists { get; set; } = [];
    public ICollection<Rating> Ratings { get; set; } = [];
    public ICollection<OtpCode> OtpCodes { get; set; } = [];
    public ICollection<UserRole> Roles { get; set; } = [];
}