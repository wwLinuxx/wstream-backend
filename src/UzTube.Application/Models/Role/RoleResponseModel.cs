namespace UzTube.Application.Models.Role;

public record RoleResponseModel : BaseResponseModel
{
    public string Name { get; init; } = null!;
}