namespace UzTube.Application.Models.Comment;

public record CommentResponseModel : BaseResponseModel
{
    public Guid PostId { get; init; }

    public Guid UserId { get; init; }

    public string CommentText { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
}