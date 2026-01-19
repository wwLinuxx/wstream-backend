using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class Stream : BaseEntity
{
    public Guid UserId { get; set; }
    public string StreamKey { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? PreviewUrl { get; set; }
    public string? VideoUrl { get; set; }
    public int LikesCount { get; set; }
    public int ViewerCount { get; set; }
    public int TotalViews { get; set; }
    public bool IsLive { get; set; }
    public bool IsPrivate { get; set; } = false;
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedOn { get; set; }

    public User User { get; set; } = null!;
}
