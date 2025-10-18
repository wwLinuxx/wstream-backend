using FluentValidation;
using FluentValidation.AspNetCore;
using UzTube.API;
using UzTube.API.Filters;
using UzTube.API.Middleware;
using UzTube.Application;
using UzTube.Application.Extensions;
using UzTube.Application.Models.Validators;
using UzTube.Application.Models.Validators.User;
using UzTube.DataAccess;
using UzTube.Models.DTO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(
    config => config.Filters.Add(typeof(ValidateModelAttribute))
);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(IValidationMarker));

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