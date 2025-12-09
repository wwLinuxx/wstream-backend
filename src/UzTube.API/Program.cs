using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Scalar.AspNetCore;
using UzTube.API;
using UzTube.API.Extensions;
using UzTube.API.Filters;
using UzTube.API.Middleware;
using UzTube.Application;
using UzTube.Application.Models.Validators;
using UzTube.DataAccess;
using UzTube.DataAccess.Persistence;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilog();

builder.Services.AddControllers(config =>
    config.Filters.Add(typeof(ValidateModelAttribute)));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(IValidationMarker));

builder.Services.AddHttpContextAccessor();

builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddScalar();
builder.Services.AddJwt(builder.Configuration);

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 10L * 1024 * 1024 * 1024;
    options.AllowSynchronousIO = true;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10L * 1024 * 1024 * 1024;
});

WebApplication app = builder.Build();

using IServiceScope scope = app.Services.CreateScope();

await AutomatedMigration.MigrateAsync(scope.ServiceProvider);

await app.SeedRolesAndPermissionsAsync();
await app.SyncPermissionsAsync();


if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
    app.UseScalar();

if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.AddCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<PerformanceMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.AddMappedExtensions();

app.Run();