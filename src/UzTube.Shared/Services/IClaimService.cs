namespace UzTube.Shared.Services;

public interface IClaimService
{
    Guid GetUserId();

    string GetClaim(string key);
}
