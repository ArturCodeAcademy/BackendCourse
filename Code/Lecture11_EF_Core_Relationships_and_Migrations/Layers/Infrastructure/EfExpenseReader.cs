using Lecture11.Application;
using Lecture11.Domain;
using Microsoft.EntityFrameworkCore;

namespace Lecture11.Infrastructure;

public sealed class EfExpenseReader : IExpenseReader
{
    private readonly ExpenseDbContext context;

    public EfExpenseReader(ExpenseDbContext context)
    {
        this.context = context;
    }

    public Task<List<Expense>> GetWithRelationsForUserAsync(int userId)
    {
        return context.Expenses
            .AsNoTracking()
            .Include(expense => expense.User)
            .Include(expense => expense.Category)
            .Where(expense => expense.UserId == userId)
            .OrderBy(expense => expense.Id)
            .ToListAsync();
    }
}
