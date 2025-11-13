namespace UzTube.Application.Common.FileStorage;

public record FileStorageSettings
{
    public string BaseUrl { get; init; } = null!;
    public string Thumbnails { get; init; } = null!;
    public string Videos { get; init; } = null!;
}