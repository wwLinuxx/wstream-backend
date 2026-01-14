namespace UzTube.Application.Models.Post;

public record UploadThumbnailFileModel
{
}

public record UploadThumbnailFileResponseModel
{
    public string PreviewUrl { get; init; } = null!;
}
