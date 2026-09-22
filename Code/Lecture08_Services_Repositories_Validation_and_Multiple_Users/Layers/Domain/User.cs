namespace Lecture08.Domain;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    // This value is a PBKDF2 record, never the password entered by the user.
    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public override string ToString() => $"{Id}: {Username}";
}
