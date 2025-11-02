using Microsoft.EntityFrameworkCore;
using UzTube.DataAccess.Persistence;

namespace UzTube.Application.Services.Impl;

public class PermissionService(DatabaseContext context) : IPermissionService
{
    public async Task<List<string>> GetUserPermissions(Guid userId)
    {
        return await context.UserRoles
            .Where(ur => ur.UserId == userId)
            .SelectMany(ur => ur.Role.RolePermissions)
            .Select(rp => rp.Permission.Name)
            .ToListAsync();
    }
}