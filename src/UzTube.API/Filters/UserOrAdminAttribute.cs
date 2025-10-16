using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Text.Json;
using UzTube.Application.Exeptions;

namespace UzTube.Attributes;

public class UserOrAdminAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        ClaimsPrincipal user = context.HttpContext.User;

        if (user.Identity == null || !user.Identity.IsAuthenticated)
            throw new UnauthorizedAccessException("Unauthorized");

        string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        string? requestUserId = context.HttpContext.Request.RouteValues["id"]?.ToString();

        string? rolesJson = user.FindFirstValue(ClaimTypes.Role);

        if (userId != null && requestUserId != null && rolesJson != null)
        {
            List<string> roles = JsonSerializer.Deserialize<List<string>>(rolesJson);

            bool hasRole = roles.Contains("Admin") || roles.Contains("root");

            if (hasRole) return;
        }

        if (userId != requestUserId)
            throw new ForbiddenException("Forbidden");
    }
}
