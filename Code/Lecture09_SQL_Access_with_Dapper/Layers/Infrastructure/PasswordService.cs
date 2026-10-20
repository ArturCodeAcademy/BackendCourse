using System.Security.Cryptography;
using Lecture09.Application;

namespace Lecture09.Infrastructure;

public class PasswordService : IPasswordService
{
    private const int Iterations = 210_000;
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const string Algorithm = "PBKDF2-SHA256";

    public string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
        return Algorithm + "$" + Iterations + "$" + Convert.ToBase64String(salt) + "$" + Convert.ToBase64String(hash);
    }

    public bool Verify(string password, string passwordHash)
    {
        string[] parts = passwordHash.Split('$');

        if (parts.Length != 4 || parts[0] != Algorithm || !int.TryParse(parts[1], out int iterations))
        {
            return false;
        }

        try
        {
            byte[] salt = Convert.FromBase64String(parts[2]);
            byte[] storedHash = Convert.FromBase64String(parts[3]);
            byte[] candidateHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, storedHash.Length);
            return CryptographicOperations.FixedTimeEquals(candidateHash, storedHash);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
