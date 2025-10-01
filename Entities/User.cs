using System.ComponentModel.DataAnnotations;

namespace UzTube.Entities;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public string PasswordHash { get; set; }
    
    public string Salt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; } = false;

    public DateTime? DeletedAt { get; set; }

    public ICollection<UserRole> Roles { get; set; }

    public UserProfile Profile { get; set; }

    public ICollection<UserFollower> Followers { get; set; }
    public ICollection<UserFollower> Following { get; set; }

    public ICollection<Post> Posts { get; set; }

    public ICollection<PostView> PostViews { get; set; }

    public ICollection<PostLike> PostLikes { get; set; }

    public ICollection<PostComment> PostComments { get; set; }
    
    public ICollection<PostCommentLike> PostCommentLikes { get; set; }

    public ICollection<UserPlaylist> UserPlaylists { get; set; }

    public ICollection<PostRating> PostRatings { get; set; }
}
