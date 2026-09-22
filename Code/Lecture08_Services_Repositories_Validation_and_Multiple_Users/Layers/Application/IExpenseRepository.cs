using Lecture08.Domain;

namespace Lecture08.Application;

public interface IExpenseRepository
{
    List<Expense> GetAll();

    List<Expense> GetByUserId(int userId);

    void Add(Expense expense);

    bool DeleteByIdForUser(int expenseId, int userId);
}
