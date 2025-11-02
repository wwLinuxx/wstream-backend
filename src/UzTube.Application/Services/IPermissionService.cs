namespace UzTube.Application.Services;

public interface IPermissionService
{
    Task<List<string>> GetUserPermissions(Guid userId);
}