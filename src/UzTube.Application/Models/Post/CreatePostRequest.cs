using Microsoft.AspNetCore.Http;

namespace UzTube.Application.Models.Post;

public record CreatePostRequest(
    string Title,
    string? Description,
    IFormFile VideoFile,
    IFormFile? PreviewFile,
    bool IsPrivate);

public record CreatePostResponseModel : BaseResponseModel;