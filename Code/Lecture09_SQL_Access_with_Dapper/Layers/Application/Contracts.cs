using Lecture09.Domain;

namespace Lecture09.Application;

public interface IUserRepository
{
    User? FindByUsername(string username);
    User? FindById(int id);
    int Add(User user);
}

public interface IExpenseRepository
{
    IReadOnlyList<Expense> GetForUser(int userId);
    int Add(Expense expense);
    bool DeleteForUser(int expenseId, int userId);
}

public interface IPasswordService
{
    string Hash(string password);
    bool Verify(string password, string passwordHash);
}
