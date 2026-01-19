namespace UzTube.Application.Models.Comment;

public record CommentResponseModel : BaseResponseModel
{
    public Guid PostId { get; init; }
    public string CommentText { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public Guid UserId { get; set; }
    public string Username { get; init; } = null!;
    public string? UserAvatarUrl { get; init; }
}