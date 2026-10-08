using Lecture11.Domain;
using Microsoft.EntityFrameworkCore;
namespace Lecture11.Infrastructure;
public class ExpenseDbContext : DbContext
{
    public DbSet<User> Users => Set<User>(); public DbSet<Category> Categories => Set<Category>(); public DbSet<Expense> Expenses => Set<Expense>();
    public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(user => user.Username).IsUnique();
        modelBuilder.Entity<Category>().HasIndex(category => category.Name).IsUnique();
        modelBuilder.Entity<Expense>().Property(expense => expense.Amount).HasPrecision(12, 2);
        modelBuilder.Entity<Expense>().HasOne(expense => expense.User).WithMany(user => user.Expenses).HasForeignKey(expense => expense.UserId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Expense>().HasOne(expense => expense.Category).WithMany(category => category.Expenses).HasForeignKey(expense => expense.CategoryId).OnDelete(DeleteBehavior.Restrict);
    }
}
