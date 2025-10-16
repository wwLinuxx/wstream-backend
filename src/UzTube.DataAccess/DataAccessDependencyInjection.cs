using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UzTube.DataAccess.Persistence;
using UzTube.DataAccess.Repositories;
using UzTube.DataAccess.Repositories.Impl;

namespace UzTube.DataAccess;

public static class DataAccessDependencyInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);

        services.AddRepositories();

        return services;
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionSQL = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContextPool<DatabaseContext>(options =>
        {
            options.UseNpgsql(connectionSQL);
        });
    }
}
