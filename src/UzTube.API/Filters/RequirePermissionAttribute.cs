using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Text.Json;
using UzTube.Application.Exeptions;

namespace UzTube.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class RequirePermissionAttribute : Attribute, IAuthorizationFilter
{
    private readonly SystemPermissions[] _requiredPermissions;

    public RequirePermissionAttribute(params SystemPermissions[] permissions)
    {
        _requiredPermissions = permissions;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (user.Identity == null || !user.Identity.IsAuthenticated)
            throw new UnauthorizedException("Unauthorized");

        if (_requiredPermissions.Contains(SystemPermissions.Authorize))
            return;

        var permissionsJson = user.FindFirstValue("permissions");

        if (string.IsNullOrEmpty(permissionsJson))
            throw new ForbiddenException("Forbidden");

        var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        List<string>? userPermissions = JsonSerializer.Deserialize<List<string>>(permissionsJson, jsonOptions);

        if (userPermissions is null || userPermissions.Count == 0)
            throw new ForbiddenException("Forbidden");

        bool hasPermission = _requiredPermissions
            .Select(p => p.ToString())
            .Any(rp => userPermissions.Contains(rp, StringComparer.OrdinalIgnoreCase));

        if (!hasPermission)
            throw new ForbiddenException("Forbidden");
    }
}