namespace UzTube.Application.Common.Stream;

public record StreamSettings
{
    public string RtmpServer { get; init; } = null!;
    public string RtmpPort { get; init; } = null!;
    public string HlsServer { get; init; } = null!;
    public string HlsPort { get; init; } = null!;
}
