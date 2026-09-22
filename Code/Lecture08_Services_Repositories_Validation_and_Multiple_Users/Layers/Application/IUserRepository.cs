using Lecture08.Domain;

namespace Lecture08.Application;

public interface IUserRepository
{
    List<User> GetAll();

    User? GetByUsername(string username);

    void Add(User user);
}
