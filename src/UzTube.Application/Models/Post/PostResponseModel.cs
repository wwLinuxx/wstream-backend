namespace UzTube.Application.Models.Post;

public record PostResponseModel : BaseResponseModel
{
    public Guid UserId { get; set; }
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string PhotoUrl { get; init; } = null!;
    public string VideoUrl { get; init; } = null!;
    public string Duration { get; init; } = null!;
    public DateTime PostedOn { get; init; }
    public DateTime UpdatedOn { get; init; }
    public int ViewsCount { get; init; }
    public int LikesCount { get; init; }
    public int Rating { get; init; }
    public bool IsPrivate { get; set; } = false;
};