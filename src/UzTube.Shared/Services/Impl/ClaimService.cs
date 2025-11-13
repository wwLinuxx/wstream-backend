using Microsoft.AspNetCore.Http;
using UzTube.Core.Common;
using UzTube.Core.Enums;

namespace UzTube.Shared.Services.Impl;

public class ClaimService(IHttpContextAccessor httpContextAccessor) : IClaimService
{
    public Guid GetUserId()
    {
        return Guid.Parse(GetClaim(CustomClaimNames.Id));
    }

    public SystemLanguages GetUserLanguage()
    {
        string userLanguage = httpContextAccessor.HttpContext?.Request.Headers["Language"].ToString() ?? string.Empty;

        SystemLanguages userLanguageIndex = userLanguage switch
        {
            "uz" => SystemLanguages.Uzbek,
            "ru" => SystemLanguages.Russian,
            "eng" => SystemLanguages.English,
            "kz" => SystemLanguages.Kazakh,
            _ => SystemLanguages.Uzbek
        };

        return userLanguageIndex;
    }

    public string GetClaim(string key)
    {
        return httpContextAccessor.HttpContext?.User?.FindFirst(key)?.Value ?? string.Empty;
    }
}