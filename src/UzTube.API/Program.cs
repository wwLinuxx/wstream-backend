using UzTube.API;
using UzTube.API.Middleware;
using UzTube.Application;
using UzTube.Application.Extensions;
using UzTube.DataAccess;
using UzTube.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddSwagger();
builder.Services.AddJwt(builder.Configuration);

builder.Services.Configure<MinioOptions>(
    builder.Configuration.GetSection("MinioConfigurations"));

var app = builder.Build();

await app.SyncPermissionsAsync();
await app.SeedDefaultRolesAsync();

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "UzTube"); });

app.UseHttpsRedirection();

app.UseCors(corsPolicyBuilder =>
    corsPolicyBuilder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
);

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<PerformanceMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
