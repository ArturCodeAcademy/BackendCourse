using Lecture10.Domain;

namespace Lecture10.Application;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task AddAsync(User user);
}
