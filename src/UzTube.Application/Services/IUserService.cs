using UzTube.Application.Models;
using UzTube.Application.Models.User;

namespace UzTube.Application.Services;

public interface IUserService
{
    Task<CreateUserResponseModel> CreateAsync(CreateUserModel model);
    Task<LoginResponseModel> LoginAsync(LoginUserModel model);
    Task<UserResponseModel> GetMeAsync();
    Task<PaginatedList<UserResponseModel>> GetUsersAsync(PageOption option);
    Task<UserResponseModel> GetUserProfileByIdAsync(Guid id);
    Task<UserResponseModel> SearchUserByQueryAsync(string query);
    Task<UpdateUserProfileResponseModel> UpdateUserProfileByIdAsync(Guid id, UpdateUserProfileModel model);
    Task<UpdateUserPasswordResponseModel> UpdateUserPasswordByIdAsync(Guid id, UpdateUserPasswordModel model);
    Task<UpdateUserRoleResponseModel> UpdateUserRoleByIdAsync(Guid id, UpdateUserRoleModel model);
    Task<DeleteUserResponseModel> DeleteUserByIdAsync(Guid id);
    Task<RestoreUserResponseModel> RestoreUserByIdAsync(Guid id);
}