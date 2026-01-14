namespace UzTube.Application.Models.Post;

public record UploadVideoFileModel
{

}

public record UploadVideoFileResponseModel
{
    public string FileUrl { get; init; } = null!;
}