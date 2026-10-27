using Lecture10.Domain;

namespace Lecture10.Application;

public sealed class UserService
{
    private readonly IUserRepository repository;

    public UserService(IUserRepository repository)
    {
        this.repository = repository;
    }

    public Task<User?> FindByUsernameAsync(string username)
    {
        return repository.GetByUsernameAsync(username.Trim());
    }

    public async Task<User> RegisterAsync(string username)
    {
        string normalizedUsername = username.Trim();

        if (normalizedUsername.Length < 3)
        {
            throw new ArgumentException("Username must contain at least 3 characters.");
        }

        if (await repository.GetByUsernameAsync(normalizedUsername) is not null)
        {
            throw new ArgumentException("This username is already taken.");
        }

        var user = new User { Username = normalizedUsername };
        await repository.AddAsync(user);
        return user;
    }
}
