using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UzTube.DataAccess.Persistence;
using UzTube.Entities;

namespace UzTube.Application.Extensions;

public static class PermissionSyncExtension
{
    public static async Task SyncPermissionsAsync(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        DatabaseContext? context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        // DB Permissions
        HashSet<Permission> dbPermissions = context.Permissions.ToHashSet();

        // Enum Permissions
        List<SystemPermissions> enumPermissions = Enum.GetValues(typeof(SystemPermissions))
            .Cast<SystemPermissions>()
            .ToList();

        // 1. Enumda bor, DB da yo‘q => qo‘shamiz
        foreach (var permission in enumPermissions)
        {
            if (!dbPermissions.Any(p => p.Id == (int)permission))
            {
                context.Permissions.Add(new Permission
                {
                    Id = (int)permission,
                    Name = permission.ToString(),
                    Description = $"{permission} - Permission"
                });

                Console.WriteLine($"Added: {permission}");
            }
        }

        // 2. DB da bor, Enumda yo‘q => o‘chiramiz
        foreach (var dbPermission in dbPermissions)
        {
            if (!enumPermissions.Any(e => (int)e == dbPermission.Id))
            {
                context.Permissions.Remove(dbPermission);

                Console.WriteLine($"Removed: {dbPermission}");
            }
        }

        // 3. Nom farq qilsa yangilaymiz
        foreach (var dbPermission in dbPermissions)
        {
            var match = enumPermissions.FirstOrDefault(e => (int)e == dbPermission.Id);

            if (match != default && dbPermission.Name != match.ToString())
            {
                dbPermission.Name = match.ToString();
                dbPermission.Description = $"{match} - Permission";

                context.Permissions.Update(dbPermission);

                Console.WriteLine($"Changed: {match}");
            }
        }

        await context.SaveChangesAsync();
    }
}
