using Lecture14.Application;
using Lecture14.Domain;
using Microsoft.EntityFrameworkCore;

namespace Lecture14.Infrastructure;

public sealed class EfExpenseRepository : IExpenseRepository
{
    private readonly ExpenseDbContext context;

    public EfExpenseRepository(ExpenseDbContext context)
    {
        this.context = context;
    }

    public Task<List<Expense>> GetForUserAsync(int userId, CancellationToken cancellationToken)
    {
        return context.Expenses
            .AsNoTracking()
            .Where(item => item.UserId == userId)
            .OrderByDescending(item => item.Id)
            .ToListAsync(cancellationToken);
    }

    public Task<Expense?> GetByIdForUserAsync(int userId, int expenseId, CancellationToken cancellationToken)
    {
        return context.Expenses
            .AsNoTracking()
            .SingleOrDefaultAsync(item => item.UserId == userId && item.Id == expenseId, cancellationToken);
    }

    public async Task<Expense> AddAsync(Expense expense, CancellationToken cancellationToken)
    {
        context.Expenses.Add(expense);
        await context.SaveChangesAsync(cancellationToken);
        return expense;
    }
}
