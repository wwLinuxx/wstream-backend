using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Text.Json;
using UzTube.Services;

namespace UzTube.Attributes;

public class UserOrAdminAttribute : Attribute, IAuthorizationFilter
{
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

        string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        string? requestUserId = context.HttpContext.Request.RouteValues["id"]?.ToString();

        string rolesJson = user.FindFirstValue(ClaimTypes.Role);

        if (userId != null && requestUserId != null && rolesJson != null)
        {
            List<string> roles = JsonSerializer.Deserialize<List<string>>(rolesJson);

            bool hasRole = roles.Contains("Admin") || roles.Contains("root");

            if (hasRole) return;
        }

        if (userId != requestUserId)
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
