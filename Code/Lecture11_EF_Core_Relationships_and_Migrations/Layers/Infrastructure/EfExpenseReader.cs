using Lecture11.Application; using Lecture11.Domain; using Microsoft.EntityFrameworkCore;
namespace Lecture11.Infrastructure;
public class EfExpenseReader : IExpenseReader
{
    private readonly ExpenseDbContext context; public EfExpenseReader(ExpenseDbContext context) => this.context = context;
    public Task<List<Expense>> GetWithCategoryForUserAsync(int userId) => context.Expenses.AsNoTracking().Include(expense => expense.Category).Where(expense => expense.UserId == userId).ToListAsync();
}
