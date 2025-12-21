using UzTube.Shared.Helpers.Interfaces;

namespace UzTube.Shared.Helpers;

public class PasswordHasher : IPasswordHasher
{
    private const int WorkFactor = 12;

    public string Hash(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);

    public bool Verify(string password, string hashedPassword) =>
        BCrypt.Net.BCrypt.Verify(password, hashedPassword);

    public bool NeedReHash(string hashedPassword) =>
        BCrypt.Net.BCrypt.PasswordNeedsRehash(hashedPassword, WorkFactor);
}