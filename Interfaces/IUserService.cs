using UzTube.Entities;

namespace UzTube.Interfaces;

public interface IUserService
{
    bool Exists(int userId);
    User GetById(int userId);
    IEnumerable<User> Users();
    IEnumerable<string> Roles();
    IEnumerable<string> Roles(int userId);
    IEnumerable<string> Permissions(int userId);
}