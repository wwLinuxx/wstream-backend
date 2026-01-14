namespace UzTube.Application.Helpers.Interfaces;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hashedPassword);
    bool NeedReHash(string hashedPassword);
}