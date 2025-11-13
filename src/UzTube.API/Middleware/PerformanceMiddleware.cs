using System.Diagnostics;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using UzTube.Application.Common.Performance;

namespace UzTube.API.Middleware;

public class PerformanceMiddleware(
    RequestDelegate next,
    IOptions<PerformanceSettings> performanceSettings,
    ILogger<PerformanceMiddleware> logger
)
{
    private readonly PerformanceSettings _performanceSettings = performanceSettings.Value;

    public async Task Invoke(HttpContext context)
    {
        int warningThresholdMs = _performanceSettings.MaxRequestTiming;
    
        Stopwatch sw = new Stopwatch();

        sw.Start();

        await next(context);

        sw.Stop();

        long elapsed = sw.ElapsedMilliseconds;
        int? statusCode = context.Response?.StatusCode;
        string user = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? "Anonymous";

        if (sw.ElapsedMilliseconds > warningThresholdMs)
            logger.LogWarning(
                "[PERFORMANCE] {Method} {Path} | {StatusCode} | {User} | {Elapsed} ms",
                context.Request.Method,
                context.Request.Path.Value,
                statusCode,
                user,
                elapsed
            );
        else 
            logger.LogInformation(
                "[PERFORMANCE] {Method} {Path} | {StatusCode} | {User} | {Elapsed} ms",
                context.Request.Method,
                context.Request.Path.Value,
                statusCode,
                user,
                elapsed
            );
    }
}