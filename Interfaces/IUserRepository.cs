using UzTube.Models.DTO;
using UzTube.Services;

namespace UzTube.Interfaces
{
    public interface IUserRepository
    {
        Result Login(LoginDTO dto);
        Result Register(RegisterDTO dto);
    }
}