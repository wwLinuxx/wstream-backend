namespace UzTube.Application.Helpers.GenerateJwt;

public record JwtSettings(
    string Issuer,
    string Audience,
    string SecretKey,
    int ExpirationInSeconds);
