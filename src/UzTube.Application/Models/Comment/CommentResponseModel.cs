namespace UzTube.Application.Models.Comment;

public record CommentResponseModel : BaseResponseModel
{
    public string CommentText { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}