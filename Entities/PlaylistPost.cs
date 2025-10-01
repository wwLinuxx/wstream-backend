namespace UzTube.Entities;

public class PlaylistPost
{
    public int PlaylistId { get; set; }
    public UserPlaylist UserPlaylist { get; set; }

    public int PostId { get; set; }
    public Post Post { get; set; }
}
