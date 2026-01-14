using UzTube.Core.Enums;

namespace UzTube.Shared.Services;

public interface IClaimService
{
    Guid GetUserId();
    public bool IsAuthenticated();
    SystemLanguages GetUserLanguage();
    string GetClaim(string key);
}
