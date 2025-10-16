using System.Diagnostics;

namespace UzTube.API.Middleware;

public class PerformanceMiddleware(RequestDelegate next, ILogger<PerformanceMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        const int performanceTimeLog = 500;

        var sw = new Stopwatch();

        sw.Start();

        await next(context);

        sw.Stop();

        if (sw.ElapsedMilliseconds > performanceTimeLog)
            logger.LogWarning("Request {method} {path} it look about {elapsed} ms",
                context.Request?.Method,
                context.Request?.Path.Value,
                sw.ElapsedMilliseconds);
    }
}
