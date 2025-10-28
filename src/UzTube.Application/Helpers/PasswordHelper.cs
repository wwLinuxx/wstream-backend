using System.Security.Cryptography;
using System.Text;
using UzTube.Application.Helpers.Interfaces;

namespace UzTube.Application.Helpers;

public class PasswordHelper : IPasswordHelper
{
    public string Encrypt(string password, string salt)
    {
        using var algorithm = new Rfc2898DeriveBytes(
            password,
            Encoding.UTF8.GetBytes(salt),
            8192,
            HashAlgorithmName.SHA256);

        return Convert.ToBase64String(algorithm.GetBytes(50));
    }

    public string GenerateSalt()
    {
        return Guid.NewGuid().ToString();
    }

    public bool Verify(string passwordHash, string password, string salt)
    {
        return passwordHash == Encrypt(password, salt);
    }
}