namespace UzTube.Application.Models.User;

public record UpdateUserRolesRequest
{
    public ISet<Guid> Roles { get; init; } = null!;
}

public record UpdateUserRoleResponseModel : BaseResponseModel;