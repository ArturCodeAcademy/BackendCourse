using Lecture10.Domain;
using Microsoft.EntityFrameworkCore;

namespace Lecture10.Infrastructure;

public sealed class ExpenseDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Expense> Expenses => Set<Expense>();

    public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(user => user.Username).IsUnique();
        modelBuilder.Entity<User>().Property(user => user.Username).HasMaxLength(50).IsRequired();

        modelBuilder.Entity<Expense>().Property(expense => expense.Name).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Expense>().Property(expense => expense.Amount).HasPrecision(12, 2);
        modelBuilder.Entity<Expense>()
            .HasOne(expense => expense.User)
            .WithMany(user => user.Expenses)
            .HasForeignKey(expense => expense.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
