namespace UzTube.Application.Models.User;

public record UpdateUserRoleModel
{
    public ISet<Guid> Roles { get; init; } = null!;
}

public record UpdateUserRoleResponseModel : BaseResponseModel;