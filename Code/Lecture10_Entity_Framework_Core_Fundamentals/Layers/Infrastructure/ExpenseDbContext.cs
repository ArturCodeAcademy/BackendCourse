using Lecture10.Domain;
using Microsoft.EntityFrameworkCore;
namespace Lecture10.Infrastructure;
public class ExpenseDbContext : DbContext
{
    public DbSet<Expense> Expenses => Set<Expense>();
    public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>().Property(expense => expense.Name).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Expense>().Property(expense => expense.Amount).HasPrecision(12, 2);
    }
}
