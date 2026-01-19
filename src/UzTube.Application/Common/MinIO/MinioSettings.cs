namespace UzTube.Application.Common.Minio;

public record MinioSettings
{
    public string Url { get; init; } = null!;
    public string Server { get; init; } = null!;
    public int Port { get; init; }
    public string AccessKey { get; init; } = null!;
    public string SecretKey { get; init; } = null!;
    public bool UseSSL { get; init; }
}