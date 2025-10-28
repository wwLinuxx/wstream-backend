namespace UzTube.Application.Models.Post;

public record CreatePostModel(
    string Title,
    string Description,
    string ThumbnailUrl,
    string VideoUrl,
    bool IsPrivate);

public record CreatePostResponseModel : BaseResponseModel;