using Microsoft.AspNetCore.Http;

namespace UzTube.Application.Models.Post;

public record UpdatePostRequest(
    string Title,
    string? Description,
    FormFile? PreviewFile,
    bool IsPrivate);

public record UpdatePostResponseModel : BaseResponseModel;