using UzTube.API;
using UzTube.API.Middleware;
using UzTube.Application;
using UzTube.Application.Extensions;
using UzTube.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddSwagger();
builder.Services.AddJwt(builder.Configuration);
builder.Services.AddMinio();

var app = builder.Build();

await app.SyncPermissionsAsync();
await app.SeedDefaultRolesAsync();

app.UseSwagger();
app.UseSwaggerUI(s => { s.SwaggerEndpoint("/swagger/v1/swagger.json", "UzTube"); });

app.UseHttpsRedirection();

app.AddCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<PerformanceMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();