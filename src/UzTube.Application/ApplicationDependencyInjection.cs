using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UzTube.Application.Common.Email;
using UzTube.Application.Common.Minio;
using UzTube.Application.Common.Otp;
using UzTube.Application.Common.Performance;
using UzTube.Application.Helpers;
using UzTube.Application.Helpers.Interfaces;
using UzTube.Application.Services;
using UzTube.Application.Services.Impl;
using UzTube.Shared.Services;
using UzTube.Shared.Services.Impl;

namespace UzTube.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServices();
        services.AddConfigurations(configuration);

        return services;
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IOtpService, OtpEmailService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IClaimService, ClaimService>();
    }

    private static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
        services.Configure<MinioSettings>(configuration.GetSection("MinioSettings"));
        services.Configure<OtpSettings>(configuration.GetSection("OtpSettings"));
        services.Configure<PerformanceSettings>(configuration.GetSection("PerformanceSettings"));
    }
}