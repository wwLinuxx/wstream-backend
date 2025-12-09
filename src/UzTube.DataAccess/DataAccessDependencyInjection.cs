using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UzTube.Core.Common;
using UzTube.Core.Entities;
using UzTube.Core.Enums;
using UzTube.DataAccess.Persistence;

namespace UzTube.DataAccess;

public static class DataAccessDependencyInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);

        return services;
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionSql = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContextPool<DatabaseContext>(options =>
            options.UseNpgsql(connectionSql));
    }

    public static async Task SeedRolesAndPermissionsAsync(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        DatabaseContext context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        ArgumentNullException.ThrowIfNull(context);

        await SeedRolesAsync(context);
        await AssignRootRoleToRootUserAsync(context);
        await GrantAllPermissionsToRootRoleAsync(context);
    }

    public static async Task SyncPermissionsAsync(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        DatabaseContext context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        ILogger? logger = scope.ServiceProvider.GetService<ILoggerFactory>()?.CreateLogger("PermissionSync");

        logger?.LogInformation("[PERMISSION SYNC] Checking for new permissions...");

        List<Permission> dbPermissions = await context.Permissions.AsNoTracking().ToListAsync();
        List<SystemPermissions> enumPermissions = Enum.GetValues<SystemPermissions>().ToList();

        HashSet<int> dbPermissionIds = dbPermissions.Select(p => p.Id).ToHashSet();
        HashSet<int> enumPermissionIds = enumPermissions.Select(e => (int)e).ToHashSet();

        // Enumda bor, DBda yo‘q — qo‘shamiz
        List<Permission> toAdd = enumPermissions
            .Where(ep => !dbPermissionIds.Contains((int)ep))
            .Select(e => new Permission
            {
                Id = (int)e,
                Name = e.ToString(),
                Description = $"{e} - Permission"
            })
            .ToList();

        if (toAdd.Count > 0)
        {
            await context.Permissions.AddRangeAsync(toAdd);

            logger?.LogInformation("[PERMISSION SYNC] Added {Count} new permissions: {Permissions}",
                toAdd.Count, string.Join(", ", toAdd.Select(p => p.Name)));
        }

        // DBda bor, Enumda yo‘q — o‘chiramiz
        List<Permission> toRemove = dbPermissions
            .Where(p => !enumPermissionIds.Contains(p.Id))
            .ToList();

        if (toRemove.Count > 0)
        {
            context.Permissions.RemoveRange(toRemove);

            logger?.LogWarning("[PERMISSION SYNC] Removed {Count} permissions: {Permissions}",
                toRemove.Count, string.Join(", ", toRemove.Select(p => p.Name)));
        }

        // Nom yoki description farq qilsa — yangilaymiz
        List<Permission> toUpdate = dbPermissions
            .Where(p =>
                enumPermissionIds.Contains(p.Id) &&
                enumPermissions.Any(e => (int)e == p.Id && p.Name != e.ToString()))
            .Select(p =>
            {
                SystemPermissions match = enumPermissions.First(e => (int)e == p.Id);
                p.Name = match.ToString();
                p.Description = $"{match} - Permission";
                p.UpdatedOn = DateTime.UtcNow;
                return p;
            })
            .ToList();

        if (toUpdate.Count > 0)
        {
            context.Permissions.UpdateRange(toUpdate);

            logger?.LogInformation("Updated {Count} permissions: {Permissions}",
                toUpdate.Count, string.Join(", ", toUpdate.Select(p => p.Name)));
        }

        if (toAdd.Count > 0 || toRemove.Count > 0 || toUpdate.Count > 0)
            await context.SaveChangesAsync();
        else
            logger?.LogInformation("[PERMISSION SYNC] All permissions are already in sync.");
    }

    private static async Task SeedRolesAsync(DatabaseContext context)
    {
        Guid rootRoleId = SystemIds.Role.Root;
        Guid adminRoleId = SystemIds.Role.Admin;
        Guid moderatorRoleId = SystemIds.Role.Moderator;
        Guid userRoleId = SystemIds.Role.User;

        if (!await context.Roles.AnyAsync(r => r.Id == rootRoleId))
            context.Roles.Add(new Role
            {
                Id = rootRoleId,
                Name = "Root",
                Description = "Root",
                CreatedOn = DateTime.UtcNow
            });

        if (!await context.Roles.AnyAsync(r => r.Id == adminRoleId))
            context.Roles.Add(new Role
            {
                Id = adminRoleId,
                Name = "Admin",
                Description = "Admin",
                CreatedOn = DateTime.UtcNow
            });

        if (!await context.Roles.AnyAsync(r => r.Id == moderatorRoleId))
            context.Roles.Add(new Role
            {
                Id = moderatorRoleId,
                Name = "Moderator",
                Description = "Moderator",
                CreatedOn = DateTime.UtcNow
            });

        if (!await context.Roles.AnyAsync(r => r.Id == userRoleId))
            context.Roles.Add(new Role
            {
                Id = userRoleId,
                Name = "User",
                Description = "User",
                CreatedOn = DateTime.UtcNow
            });

        await context.SaveChangesAsync();
    }

    private static async Task AssignRootRoleToRootUserAsync(DatabaseContext context)
    {
        Guid rootUserId = SystemIds.User.Root;
        Guid rootRoleId = SystemIds.Role.Root;

        bool rootUserHasAdminRole = await context.UserRoles
            .AnyAsync(ur => ur.UserId == rootUserId && ur.RoleId == rootRoleId);

        if (!rootUserHasAdminRole)
            context.UserRoles.Add(new UserRole
            {
                UserId = rootUserId,
                RoleId = rootRoleId
            });

        await context.SaveChangesAsync();
    }

    private static async Task GrantAllPermissionsToRootRoleAsync(DatabaseContext context)
    {
        Guid rootRoleId = SystemIds.Role.Root;

        List<Permission>? missingRootRolePermissions = await context.Permissions
            .Where(p => !context.RolePermissions
                .Any(rp => rp.RoleId == rootRoleId && rp.PermissionId == p.Id))
            .ToListAsync();

        if (missingRootRolePermissions.Count > 0)
        {
            var newRootRolePermissions = missingRootRolePermissions
                .Select(p => new RolePermission
                {
                    RoleId = rootRoleId,
                    PermissionId = p.Id
                });

            await context.RolePermissions.AddRangeAsync(newRootRolePermissions);
            await context.SaveChangesAsync();
        }
    }
}