using Lecture14.Domain;
using Microsoft.EntityFrameworkCore;

namespace Lecture14.Infrastructure;

public class ExpenseDbContext : DbContext
{
    public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options)
        : base(options)
    {
    }

    public DbSet<Expense> Expenses => Set<Expense>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Expense>()
            .Property(item => item.Amount)
            .HasPrecision(12, 2);

        builder.Entity<Expense>()
            .HasIndex(item => new { item.UserId, item.Id });
    }
}