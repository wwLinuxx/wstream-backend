namespace UzTube.Application.Models.Post;

public record UploadVideoModel
{

}

public record UploadVideoFileResponseModel
{
    public string FileUrl { get; init; } = null!;
}