using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Text.Json;
using UzTube.Application.Exceptions;
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
        ClaimsPrincipal user = context.HttpContext.User;

        // Foydalanuvchi tizimga kirmagan bo‘lsa
        if (user.Identity is not { IsAuthenticated: true })
            throw new UnauthorizedException("User is not authenticated");

        // Foydalanuvchi permissions claimi yo‘q bo‘lsa
        string? permissionsJson = user.FindFirstValue("permissions");
        if (string.IsNullOrEmpty(permissionsJson))
            throw new ForbiddenException("User has not permissions");

        // JSONdan deserialize qilamiz
        List<string>? userPermissions = JsonSerializer.Deserialize<List<string>>(permissionsJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (userPermissions is null || userPermissions.Count == 0)
            throw new ForbiddenException("No permissions assigned");

        // Kerakli ruxsatlar mavjudligini tekshiramiz
        bool hasPermission = _permissions
            .Select(p => p.ToString())
            .Any(required => userPermissions.Contains(required, StringComparer.OrdinalIgnoreCase));

        if (!hasPermission)
            throw new ForbiddenException("Access denied");
    }
}