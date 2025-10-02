using UzTube.Models.DTO;
using UzTube.Services;

namespace UzTube.Interfaces;

public interface IUserRepository
{
    Task<Result> Login(LoginDTO dto);
    Task<Result> Register(RegisterDTO dto);
    Task<Result<UserGetDTO>> Me();
    Task<Result<List<UserGetDTO>>> GetAllUsersAsync();
    Task<Result<UserGetDTO>> GetUserProfileByIdAsync(int id);
    Task<Result<UserGetDTO>> SearchUserByQueryAsync(int id);
    Task<Result> UpdateUserProfileByIdAsync(int id, UserProfileUpdateDTO dto);
    Task<Result> UpdateUserPasswordByIdAsync(int id, UserPasswordUpdateDTO dto);
    Task<Result> UpdateUserRoleByIdAsync(int id, UserRoleUpdateDTO dto);
    Task<Result> DeleteUserByIdAsync(int id);
    Task<Result> RestoreUserByIdAsync(int id);
}