namespace UzTube.Application.Models.User;

public record UserResponseModel : BaseResponseModel
{
    public string Username { get; init; } = null!;
    public string Email { get; init; } = null!;
    public Guid CountryId { get; init; }
    public DateTime CreatedOn { get; init; }
    public DateTime? UpdatedOn { get; init; }

    public IReadOnlyCollection<string> Roles { get; init; } = [];
}