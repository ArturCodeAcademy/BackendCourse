using Lecture08.Domain;

namespace Lecture08.Application;

public class AuthService
{
    private readonly IUserRepository users;
    private readonly IPasswordService passwords;

    public AuthService(IUserRepository users, IPasswordService passwords)
    {
        this.users = users;
        this.passwords = passwords;
    }

    public bool Register(string? username, string? password, out string message)
    {
        username = username?.Trim() ?? string.Empty;
        password ??= string.Empty;

        if (username.Length < 3)
        {
            message = "Username must contain at least 3 characters.";
            return false;
        }

        if (password.Length < 8)
        {
            message = "Password must contain at least 8 characters.";
            return false;
        }

        if (users.GetByUsername(username) is not null)
        {
            message = "This username is already taken.";
            return false;
        }

        List<User> allUsers = users.GetAll();
        int nextId = allUsers.Count == 0 ? 1 : allUsers.Max(user => user.Id) + 1;

        var user = new User
        {
            Id = nextId,
            Username = username,
            PasswordHash = passwords.Hash(password),
            CreatedAt = DateTime.UtcNow
        };

        users.Add(user);
        message = "User '" + user.Username + "' was registered.";
        return true;
    }

    public User? Login(string? username, string? password, out string message)
    {
        username = username?.Trim() ?? string.Empty;
        password ??= string.Empty;

        User? user = users.GetByUsername(username);

        // Use one answer for both failures: it does not reveal whether a username exists.
        if (user is null || !passwords.Verify(password, user.PasswordHash))
        {
            message = "Invalid username or password.";
            return null;
        }

        message = "Welcome, " + user.Username + ".";
        return user;
    }
}

