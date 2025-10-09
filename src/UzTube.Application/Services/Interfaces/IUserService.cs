using UzTube.Models;
using UzTube.Models.DTO;

namespace UzTube.Interfaces;

public interface IUserService
{
    Task<ApiResult> Login(LoginDTO dto);

    Task<ApiResult> Register(RegisterDTO dto);

    Task<Result<UserGetDTO>> Me();

    Task<Result<List<UserGetDTO>>> GetAllUsersAsync();

    Task<Result<UserGetDTO>> GetUserProfileByIdAsync(int id);

    Task<Result<UserGetDTO>> SearchUserByQueryAsync(int id);

    Task<ApiResult> UpdateUserProfileByIdAsync(int id, UserProfileUpdateDTO dto);

    Task<ApiResult> UpdateUserPasswordByIdAsync(int id, UserPasswordUpdateDTO dto);

    Task<ApiResult> UpdateUserRoleByIdAsync(int id, UserRoleUpdateDTO dto);

    Task<ApiResult> DeleteUserByIdAsync(int id);

    Task<ApiResult> RestoreUserByIdAsync(int id);
}