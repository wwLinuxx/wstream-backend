namespace UzTube.Application.Models.Post;

public record UpdatePostModel(
    string Title,
    string Description,
    string ThumbnailUrl,
    bool IsPrivate);

public record UpdatePostResponseModel : BaseResponseModel;