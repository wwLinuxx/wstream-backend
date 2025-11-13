namespace UzTube.Application.Common.Otp;

public record OtpSettings
{
    public int Length { get; init; }
    public int ExpireMinutes { get; init; }
    public int MaxAttempts { get; init; }
    public bool RemoveAfterVerify { get; init; }
}