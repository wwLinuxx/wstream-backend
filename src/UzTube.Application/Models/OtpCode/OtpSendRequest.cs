namespace UzTube.Application.Models.OtpCode;

public record OtpSendRequest
{
    public string Email { get; set; } = null!;
}