namespace UzTube.Application.Models.Comment;

public record CreateCommentRequest
{
    public string CommentText { get; init; } = null!;
}

public record CreateCommentResponseModel : BaseResponseModel;