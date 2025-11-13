using Microsoft.Extensions.Hosting;

namespace UzTube.Application.Services.Background;

public class Notification : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine(1);
        
        return Task.CompletedTask;
    }
}