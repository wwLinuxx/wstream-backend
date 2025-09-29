using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;
using UzTube.Services;

namespace UzTube.Attributes;

public class RequirePermissionAttribute : Attribute, IAuthorizationFilter
{
    private readonly SystemPermissions[] _permissions;

    public RequirePermissionAttribute(params SystemPermissions[] permissions)
    {
        _permissions = permissions;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        ClaimsPrincipal user = context.HttpContext.User;

        if (user.Identity == null || !user.Identity.IsAuthenticated)
        {
            context.Result = new Result
            {
                Message = "Unauthorized",
                StatusCode = 401
            };

            return;
        }

        if (_permissions.Contains(SystemPermissions.Authorize))
            return;

        string permissionsJson = user.FindFirstValue("permissions");

        List<string> permissions = JsonSerializer.Deserialize<List<string>>(permissionsJson);

        bool hasPermission = permissions.Any(p => permissions.Contains(p.ToString()));

        if (permissionsJson.IsNullOrEmpty() || !hasPermission)
        {
            context.Result = new Result
            {
                Message = "Forbidden",
                StatusCode = 403
            };

            return;
        }
    }
}
