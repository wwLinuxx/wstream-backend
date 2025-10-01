namespace UzTube.Entities;

public class UserPlaylist
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public string Name { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsPrivate { get; set; } = true;

    public ICollection<PlaylistPost> PlaylistPosts { get; set; }
}
