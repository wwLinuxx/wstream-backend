namespace UzTube.Application.Models.Post;

public record PostResponseModel : BaseResponseModel
{
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public string VideoUrl { get; init; } = null!;
    public string? PreviewUrl { get; init; }
    public string Duration { get; init; } = null!;
    public DateTime PostedOn { get; init; }
    public DateTime UpdatedOn { get; init; }
    public int ViewsCount { get; init; }
    public int LikesCount { get; init; }
    public int Rating { get; init; }
    public bool IsPrivate { get; set; } = false;

    public Guid UserId { get; set; }
    public string Username { get; init; } = null!;
    public string? UserAvatarUrl { get; init; }
};