namespace UzTube.Application.Models.Minio;

public record MinioOptions(
    string Endpoint,
    string AccessKey,
    string SecretKey,
    bool UseSsl);