using UzTube.Models.DTO;
using UzTube.Services;

namespace UzTube.Interfaces;

public interface IUserRepository
{
    Task<Result> Login(LoginDTO dto);
    Task<Result> Register(RegisterDTO dto);
    Task<Result<UserGetDTO>> UserProfile(int userId);
}