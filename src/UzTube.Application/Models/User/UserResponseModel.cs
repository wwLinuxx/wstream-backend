namespace UzTube.Application.Models.User;

public record UserResponseModel : BaseResponseModel
{
    public string Email { get; init; } = null!;
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? PhoneNumber { get; init; }
    public int? Age { get; init; }
    public Guid? CountryId { get; init; }
    public string CreatedOn { get; init; } = null!;
    public string? UpdatedOn { get; init; }
    public IReadOnlyCollection<string> Roles { get; init; } = [];
}