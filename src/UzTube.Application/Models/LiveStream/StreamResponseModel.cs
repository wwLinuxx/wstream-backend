namespace UzTube.Application.Models.LiveStream;

public record StreamResponseModel : BaseResponseModel
{
    public string Title { get; init; } = null!;
    public string? Description { get; init; } = null!;
    public string StreamUrl { get; init; } = null!;
    public string? PreviewUrl { get; init; }
    public bool IsLive { get; init; }
    public int ViewerCount { get; init; }
    public DateTime? StartedAt { get; init; }

    public Guid UserId { get; init; }
    public string Username { get; init; } = null!;
    public string? UserAvatarUrl { get; init; }
}