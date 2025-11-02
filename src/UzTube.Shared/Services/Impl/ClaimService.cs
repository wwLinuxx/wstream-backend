using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace UzTube.Shared.Services.Impl;

public class ClaimService(IHttpContextAccessor httpContextAccessor) : IClaimService
{
    public Guid GetUserId()
        => Guid.Parse(GetClaim(ClaimTypes.NameIdentifier));

    public string GetClaim(string key)
        => httpContextAccessor.HttpContext?.User?.FindFirst(key)?.Value ?? string.Empty;
}