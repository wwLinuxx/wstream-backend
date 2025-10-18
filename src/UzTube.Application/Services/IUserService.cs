using UzTube.Application.Models;
using UzTube.Application.Models.User;
using UzTube.Models.DTO;

namespace UzTube.Interfaces;

public interface IUserService
{
    Task<CreateUserResponseModel> CreateAsync(CreateUserModel dto);

    Task<LoginResponseModel> LoginAsync(LoginUserModel dto);

    Task<UserResponseModel> GetMeAsync();

    Task<List<UserResponseModel>> GetUsersAsync();

    Task<PaginatedList<UserListResponseModel>> GetUsersListAsync(PageOption option);

    Task<UserResponseModel> GetUserProfileByIdAsync(Guid id);

    Task<UserResponseModel> SearchUserByQueryAsync(Guid id);

    Task<UpdateUserProfileResponseModel> UpdateUserProfileByIdAsync(Guid id, UpdateUserProfileModel dto);

    Task<UpdateUserPasswordResponseModel> UpdateUserPasswordByIdAsync(Guid id, UpdateUserPasswordModel dto);

    Task<UpdateUserRoleResponseModel> UpdateUserRoleByIdAsync(Guid id, UpdateUserRoleModel dto);

    Task<DeleteUserResonseModel> DeleteUserByIdAsync(Guid id);

    Task<RestoreUserResponseModel> RestoreUserByIdAsync(Guid id);
}