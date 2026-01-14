using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
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
        services.AddMinio(configuration);

        return services;
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IFileStorageService, MinioFileStorageService>();
        services.AddScoped<IOtpService, OtpEmailService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<IViewService, ViewService>();
        services.AddScoped<ILikeService, LikeService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICountryService, CountryService>();
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

    private static void AddMinio(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMinioClient>(sp =>
        {
            MinioSettings? config = sp.GetRequiredService<IOptions<MinioSettings>>().Value;

            IMinioClient? client = new MinioClient()
                .WithEndpoint(config.Endpoint, config.Port)
                .WithCredentials(config.AccessKey, config.SecretKey);

            if (config.UseSSL)
                client = client.WithSSL();

            return client.Build();
        });
    }
}