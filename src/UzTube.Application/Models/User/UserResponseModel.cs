using UzTube.Application.Models.Role;

namespace UzTube.Application.Models.User;

public record UserResponseModel : BaseResponseModel
{
    public string Email { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string? PhoneNumber { get; init; }
    public int Age { get; init; }
    public string CountryCode { get; init; } = null!;
    public string CreatedOn { get; init; } = null!;
    public string? UpdatedOn { get; init; }
}