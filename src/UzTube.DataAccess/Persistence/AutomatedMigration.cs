using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace UzTube.DataAccess.Persistence;

public static class AutomatedMigration
{
    public static async Task MigrateAsync(IServiceProvider service)
    {
        DatabaseContext? context = service.GetRequiredService<DatabaseContext>();

        if (context.Database.IsNpgsql())
            await context.Database.MigrateAsync();
    }
}