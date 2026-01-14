using UzTube.Application.Models;
using UzTube.Application.Models.User;

namespace UzTube.Application.Services;

public interface IUserService
{
    Task<UserResponseModel> GetProfileAsync();

    Task<UserResponseModel> GetUserAsync(Guid id);

    Task<UserResponseModel> GetUserAsync(string username);

    Task<UserPreviewResponseModel> GetUserPreviewAsync(Guid id);

    Task<PaginatedList<UserResponseModel>> GetUsersAsync(PageOption option);

    Task<UserResponseModel> SearchUserAsync(string query);

    Task<UpdateUserProfileResponseModel> UpdateUserProfileAsync(Guid id, UpdateUserRequest request);

    Task<UpdateUserPasswordResponseModel> UpdateUserPasswordAsync(Guid id, UpdateUserPasswordRequest request);

    Task<UpdateUserRoleResponseModel> UpdateUserRolesAsync(Guid id, UpdateUserRolesRequest request);

    Task<DeleteUserResponseModel> DeleteUserAsync(Guid id);

    Task<RestoreUserResponseModel> RestoreUserAsync(Guid id);
}