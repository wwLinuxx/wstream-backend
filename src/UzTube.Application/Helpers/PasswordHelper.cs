using System.Security.Cryptography;
using System.Text;
using UzTube.Application.Helpers.Interfaces;

namespace UzTube.Application.Helpers;

public class PasswordHelper : IPasswordHelper
{
    public string Encrypt(string password, string salt)
    {
        using Rfc2898DeriveBytes algorithm = new Rfc2898DeriveBytes(
            password: password,
            salt: Encoding.UTF8.GetBytes(salt),
            iterations: 8192,
            hashAlgorithm: HashAlgorithmName.SHA256);

        return Convert.ToBase64String(algorithm.GetBytes(50));
    }

    public string GenerateSalt()
    {
        return Guid.NewGuid().ToString()[..16];
    }

    public bool Verify(string passwordHash, string password, string salt)
    {
        return passwordHash == Encrypt(password, salt);
    }
}
