using UzTube.Entities;

namespace UzTube.Interfaces;

public interface IUserService
{
    bool Exists(int userId);
    bool Exists(string email);
    User GetById(int userId);
    User GetByEmail(string email);
    IEnumerable<User> Users();
    IEnumerable<string> Roles();
    IEnumerable<string> Roles(int userId);
    IEnumerable<string> Permissions(int userId);
}