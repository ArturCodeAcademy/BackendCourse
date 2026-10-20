using Lecture09.Domain;

namespace Lecture09.Application;

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

        if (users.FindByUsername(username) is not null)
        {
            message = "This username is already taken.";
            return false;
        }

        int id = users.Add(new User
        {
            Username = username,
            PasswordHash = passwords.Hash(password)
        });

        message = "User '" + username + "' was registered with id " + id + ".";
        return true;
    }

    public User? Login(string? username, string? password, out string message)
    {
        username = username?.Trim() ?? string.Empty;
        password ??= string.Empty;

        User? user = users.FindByUsername(username);

        if (user is null || !passwords.Verify(password, user.PasswordHash))
        {
            message = "Invalid username or password.";
            return null;
        }

        message = "Welcome, " + user.Username + ".";
        return user;
    }
}
