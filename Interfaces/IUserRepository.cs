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
    Task<Result> UpdateUserProfileById(int id, UserProfileUpdateDTO dto);
    Task<Result> UpdateUserPasswordById(int id, UserPasswordUpdateDTO dto);
}