using Microsoft.Extensions.DependencyInjection;
using UzTube.Application.Helpers;
using UzTube.Application.Helpers.Interfaces;
using UzTube.Application.Services;
using UzTube.Application.Services.Impl;
using UzTube.Shared.Services;
using UzTube.Shared.Services.Impl;

namespace UzTube.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddServices();

        return services;
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IPasswordHelper, PasswordHelper>();
        services.AddScoped<IClaimService, ClaimService>();
    }
}