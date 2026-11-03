using Lecture13.Application;
using Lecture13.Domain;

namespace Lecture13.Infrastructure;

public sealed class InMemoryExpenseRepository : IExpenseRepository
{
    private readonly List<Expense> items = [];

    public IReadOnlyList<Expense> GetForUser(int userId)
    {
        return items.Where(item => item.UserId == userId).ToList();
    }

    public Expense? GetByIdForUser(int userId, int expenseId)
    {
        return items.SingleOrDefault(item => item.UserId == userId && item.Id == expenseId);
    }

    public Expense Add(Expense expense)
    {
        expense.Id = items.Count == 0 ? 1 : items.Max(item => item.Id) + 1;
        items.Add(expense);
        return expense;
    }
}
