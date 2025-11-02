using System.Diagnostics;
using System.Security.Claims;

namespace UzTube.API.Middleware;

public class PerformanceMiddleware(
    RequestDelegate next,
    ILogger<PerformanceMiddleware> logger
)
{
    public async Task Invoke(HttpContext context)
    {
        const int warningThresholdMs = 500;

        var sw = new Stopwatch();

        sw.Start();

        await next(context);

        sw.Stop();

        var elapsed = sw.ElapsedMilliseconds;
        var statusCode = context.Response?.StatusCode;
        var user = context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? "Anonymous";

        if (sw.ElapsedMilliseconds > warningThresholdMs)
            logger.LogWarning(
                "[PERFORMANCE] {Method} {Path} | {StatusCode} | {User} | {Elapsed} ms",
                context.Request?.Method,
                context.Request?.Path.Value,
                statusCode,
                user,
                elapsed
            );
        else
            logger.LogInformation(
                "[PERFORMANCE] {Method} {Path} | {StatusCode} | {User} | {Elapsed} ms",
                context.Request?.Method,
                context.Request?.Path.Value,
                statusCode,
                user,
                elapsed
            );
    }
}