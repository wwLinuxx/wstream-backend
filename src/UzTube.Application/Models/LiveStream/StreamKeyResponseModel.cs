namespace UzTube.Application.Models.LiveStream;

public record StreamKeyResponseModel
{
    public Guid StreamId { get; init; }
    public string StreamKey { get; init; } = null!;
    public string RmtpServer { get; init; } = null!;
}