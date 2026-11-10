using Lecture13.Domain;

namespace Lecture13.Application;

public interface IExpenseRepository
{
    IReadOnlyList<Expense> GetForUser(int userId);

    Expense? GetByIdForUser(int userId, int expenseId);

    Expense Add(Expense expense);
}