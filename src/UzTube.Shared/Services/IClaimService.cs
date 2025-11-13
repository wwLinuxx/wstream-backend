using UzTube.Core.Enums;

namespace UzTube.Shared.Services;

public interface IClaimService
{
    Guid GetUserId();
    SystemLanguages GetUserLanguage();
    string GetClaim(string key);
}
