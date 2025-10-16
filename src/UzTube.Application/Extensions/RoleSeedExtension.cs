using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UzTube.DataAccess.Persistence;
using UzTube.Entities;

namespace UzTube.Application.Extensions;

public static class RoleSeedExtension
{
    public static async Task SeedDefaultRolesAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        // DB dagi mavjud rollar
        HashSet<string> dbRoles = context.Roles
            .Select(r => r.Name)
            .ToHashSet();

        // Default rollar
        List<Role> defaultRoles = new()
            {
                new Role
                {
                    Id = 1,
                    Name = "root",
                    Description = "root - Role",
                    RolePermissions = new List<RolePermission>
                    {
                        new() { RoleId = 1, PermissionId = (int)SystemPermissions.ViewUsers },
                        new() { RoleId = 1, PermissionId = (int)SystemPermissions.ManageUser },
                        new() { RoleId = 1, PermissionId = (int)SystemPermissions.ViewRoles },
                        new() { RoleId = 1, PermissionId = (int)SystemPermissions.ManageRoles },
                        new() { RoleId = 1, PermissionId = (int)SystemPermissions.ViewPermissions },
                        new() { RoleId = 1, PermissionId = (int)SystemPermissions.ViewAuditLogs },
                        new() { RoleId = 1, PermissionId = (int)SystemPermissions.ManageSystem }
                    }
                },
                new Role
                {
                    Id = 2,
                    Name = "Admin",
                    Description = "Admin - Role",
                    RolePermissions = new List<RolePermission>
                    {
                        new() { RoleId = 2, PermissionId = (int)SystemPermissions.ViewUsers },
                        new() { RoleId = 2, PermissionId = (int)SystemPermissions.ManageUser },
                        new() { RoleId = 2, PermissionId = (int)SystemPermissions.ViewRoles },
                        new() { RoleId = 2, PermissionId = (int)SystemPermissions.ManageRoles }
                    }
                }
            };

        bool hasChanges = false;

        foreach (var role in defaultRoles)
        {
            if (!dbRoles.Contains(role.Name))
            {
                context.Roles.Add(role);

                Console.WriteLine($"Added default role: {role.Name}");

                hasChanges = true;
            }
        }

        if (hasChanges)
            await context.SaveChangesAsync();
    }
}
