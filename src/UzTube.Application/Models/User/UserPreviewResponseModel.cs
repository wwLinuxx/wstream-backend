namespace UzTube.Application.Models.User;

public record UserPreviewResponseModel
{
    public Guid Id { get; init; }

    public string Username { get; init; } = null!;
    
    public string Email { get; init; } = null!;
    
    public string? AvatarUrl { get; init; }
}
