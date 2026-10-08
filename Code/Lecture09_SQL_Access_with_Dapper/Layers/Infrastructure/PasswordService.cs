using System.Security.Cryptography;

namespace Lecture09.Infrastructure;

public class PasswordService
{
    public string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 210_000, HashAlgorithmName.SHA256, 32);
        return "PBKDF2-SHA256$210000$" + Convert.ToBase64String(salt) + "$" + Convert.ToBase64String(hash);
    }
}
