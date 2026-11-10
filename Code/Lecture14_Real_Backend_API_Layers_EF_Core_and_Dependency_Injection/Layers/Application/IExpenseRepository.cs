using Lecture14.Domain;

namespace Lecture14.Application;

public interface IExpenseRepository
{
    Task<List<Expense>> GetForUserAsync(int userId, CancellationToken cancellationToken);

    Task<Expense?> GetByIdForUserAsync(int userId, int expenseId, CancellationToken cancellationToken);

    Task<Expense> AddAsync(Expense expense, CancellationToken cancellationToken);
}