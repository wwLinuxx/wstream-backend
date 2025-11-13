using System.Security.Claims;
using System.Text.Json;
using UzTube.Application.Exceptions;
using UzTube.Core.Enums;

namespace UzTube.API.Extensions;

public static class RequirePermissionExtension
{
    public static RouteHandlerBuilder RequirePermission(
        this RouteHandlerBuilder endpoint,
        params SystemPermissions[] permissions)
    {
        return endpoint.AddEndpointFilter(async (context, next) =>
        {
            ClaimsPrincipal user = context.HttpContext.User;

            if (user.Identity is not { IsAuthenticated: true })
                throw new UnauthorizedException("User is not authenticated");

            string? permissionsJson = user.FindFirstValue("permissions");

            if (string.IsNullOrEmpty(permissionsJson))
                throw new ForbiddenException("User has not permissions");

            List<string>? userPermissions = JsonSerializer.Deserialize<List<string>>(permissionsJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (userPermissions is null || userPermissions.Count == 0)
                throw new ForbiddenException("No permissions assigned");

            if (permissions.Length == 0)
                return await next(context);

            bool hasPermission = permissions
                .Select(p => p.ToString())
                .Any(required => userPermissions.Contains(required, StringComparer.OrdinalIgnoreCase));

            if (!hasPermission)
                throw new ForbiddenException("Access denied");

            return await next(context);
        });
    }
}