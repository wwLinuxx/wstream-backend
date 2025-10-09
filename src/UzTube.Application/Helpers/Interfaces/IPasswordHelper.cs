namespace UzTube.Application.Helpers.Interfaces;

public interface IPasswordHelper
{
    public string Encrypt(string password, string salt);
    public string GenerateSalt();
    public bool Verify(string hashedPassword, string password, string salt);
}