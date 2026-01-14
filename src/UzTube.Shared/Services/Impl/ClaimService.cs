using Microsoft.AspNetCore.Http;
using UzTube.Core.Common;
using UzTube.Core.Enums;

namespace UzTube.Shared.Services.Impl;

public class ClaimService(IHttpContextAccessor httpContextAccessor) : IClaimService
{
    public Guid GetUserId()
    {
        if (!IsAuthenticated())
            return Guid.Empty;

        string? value = GetClaim(CustomClaimNames.Id);

        if (Guid.TryParse(value, out Guid userId))
            return userId;

        return Guid.Empty;
    }


    public bool IsAuthenticated()
    {
        return httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;
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