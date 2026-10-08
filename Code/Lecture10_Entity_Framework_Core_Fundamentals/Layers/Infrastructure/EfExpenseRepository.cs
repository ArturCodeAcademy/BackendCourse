using Lecture10.Application;
using Lecture10.Domain;
using Microsoft.EntityFrameworkCore;
namespace Lecture10.Infrastructure;
public class EfExpenseRepository : IExpenseRepository
{
    private readonly ExpenseDbContext context;
    public EfExpenseRepository(ExpenseDbContext context) => this.context = context;
    public Task<List<Expense>> GetForUserAsync(int userId) => context.Expenses.Where(expense => expense.UserId == userId).OrderByDescending(expense => expense.Id).ToListAsync();
    public async Task AddAsync(Expense expense) { context.Expenses.Add(expense); await context.SaveChangesAsync(); }
}
