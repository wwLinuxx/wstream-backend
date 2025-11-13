namespace UzTube.Application.Common.Email;

public record SmtpSettings
{
    public string Server { get; init; } = null!;
    public int Port { get; init; }
    public bool UseSsl { get; init; }
    public string SenderName { get; init; } = null!;
    public string SenderEmail { get; init; } = null!;
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
}