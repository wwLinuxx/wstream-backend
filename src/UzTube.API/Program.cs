using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;
using UzTube.API;
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

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(IValidationMarker));

builder.Services.AddHttpContextAccessor();

builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddSwagger();
builder.Services.AddJwt(builder.Configuration);
builder.Services.AddMinio();

WebApplication app = builder.Build();

using IServiceScope scope = app.Services.CreateScope();

await AutomatedMigration.MigrateAsync(scope.ServiceProvider);

await app.SeedRolesAndPermissionsAsync();
await app.SyncPermissionsAsync();

app.UseSwagger();
app.UseSwaggerUI(s => { s.SwaggerEndpoint("/swagger/v1/swagger.json", "UZTUBE"); });

app.UseHttpsRedirection();

app.AddCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<PerformanceMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();