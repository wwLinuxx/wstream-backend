using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using UzTube.Application.Exeptions;
using UzTube.Core.Enums;

namespace UzTube.API.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequirePermissionAttribute : Attribute, IAuthorizationFilter
{
    private readonly SystemPermissions[] _permissions;

    public RequirePermissionAttribute(params SystemPermissions[] permissions) =>
        _permissions = permissions;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        // 1️⃣ Foydalanuvchi tizimga kirmagan bo‘lsa
        if (user.Identity is not { IsAuthenticated: true })
            throw new UnauthorizedException("User is not authenticated");

        // 2️⃣ Foydalanuvchi permissions claimi yo‘q bo‘lsa
        var permissionsJson = user.FindFirstValue("permissions");
        if (string.IsNullOrEmpty(permissionsJson))
            throw new ForbiddenException("User has not permissions");

        // 3️⃣ JSONdan deserialize qilamiz
        var userPermissions = JsonSerializer.Deserialize<List<string>>(permissionsJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (userPermissions is null || userPermissions.Count == 0)
            throw new ForbiddenException("No permissions assigned");

        // 4️⃣ Kerakli ruxsatlar mavjudligini tekshiramiz
        var hasPermission = _permissions
            .Select(p => p.ToString())
            .Any(required => userPermissions.Contains(required, StringComparer.OrdinalIgnoreCase));

        if (!hasPermission)
            throw new ForbiddenException("Access denied");
    }
}