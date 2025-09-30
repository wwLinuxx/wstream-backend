using UzTube.Models.DTO;
using UzTube.Services;

namespace UzTube.Interfaces;

public interface IUserRepository
{
    Task<Result> Login(LoginDTO dto);
    Task<Result> Register(RegisterDTO dto);
    Task<Result<UserGetDTO>> Me();
    Task<Result<List<UserGetDTO>>> GetAllUsers();
    Task<Result<UserGetDTO>> GetUserProfileById(int id);
    Task<Result<UserGetDTO>> SearchUserByQuery(int id);
    Task<Result> UpdateUserProfileById(int id, UserProfileUpdateDTO dto);
    Task<Result> UpdateUserPasswordById(int id, UserPasswordUpdateDTO dto);
    Task<Result> UpdateUserRoleById(int id, UserRoleUpdateDTO dto);
    Task<Result> DeleteUserById(int id);
    Task<Result> RestoreUserById(int id);
}