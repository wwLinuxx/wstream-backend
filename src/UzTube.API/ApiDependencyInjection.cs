using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Minio;
using Serilog;
using UzTube.Application.Common.Minio;
using UzTube.Application.Helpers.GenerateJwt;

namespace UzTube.API;

public static class ApiDependencyInjection
{
    public static void AddSerilog(this ConfigureHostBuilder hosts)
    {
        hosts.UseSerilog((context, loggerConfiguration) => { loggerConfiguration.ReadFrom.Configuration(context.Configuration); });
    }

    public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        JwtSettings? jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

        byte[] key = Encoding.UTF8.GetBytes(jwtSettings!.SecretKey);

        services.AddAuthentication(s =>
            {
                s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(s =>
            {
                s.RequireHttpsMetadata = false;
                s.SaveToken = true;
                s.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience
                };
            });
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(s =>
        {
            s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer YOUR_TOKEN')",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            s.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    public static void AddMinio(this IServiceCollection services)
    {
        services.AddSingleton<IMinioClient>(sp =>
        {
            MinioSettings minioSettings = sp.GetRequiredService<IOptions<MinioSettings>>().Value;

            IMinioClient? client = new MinioClient()
                .WithEndpoint(minioSettings.Endpoint)
                .WithCredentials(minioSettings.AccessKey, minioSettings.SecretKey);

            // Agar SSL yoqilgan bo'lsa
            if (minioSettings.UseSsl) client = client.WithSSL();

            return client.Build(); // MinioClient ni qurish
        });
    }

    public static void AddCors(this IApplicationBuilder app)
    {
        app.UseCors(corsPolicyBuilder =>
            corsPolicyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
        );
    }
}