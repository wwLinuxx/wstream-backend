namespace UzTube.Application.Models;

public record PageOption
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string Search { get; init; } = null!;
}