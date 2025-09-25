namespace UzTube.Interfaces;

public interface IPasswordHasher
{
    public string Encrypt(string password, string salt);
    public bool Verify(string passwordHash, string password, string salt);
}