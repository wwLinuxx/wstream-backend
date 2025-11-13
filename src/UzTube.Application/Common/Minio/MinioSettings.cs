namespace UzTube.Application.Common.Minio;

public record MinioSettings
{
    public string Endpoint { get; init; }
    public string AccessKey { get; init; }
    public string SecretKey { get; init; }
    public bool UseSsl { get; init; }
}