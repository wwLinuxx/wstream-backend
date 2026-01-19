using Microsoft.AspNetCore.Http;

namespace UzTube.Application.Models.LiveStream;

public record CreateStreamRequest
{
    public string Title { get; init; } = null!;
    public string? Description { get; init; } = null;
    public IFormFile? PreviewFile { get; init; } = null;
    public bool IsPrivate { get; init; } = false;
}

public record CreateStreamResponseModel : BaseResponseModel;
