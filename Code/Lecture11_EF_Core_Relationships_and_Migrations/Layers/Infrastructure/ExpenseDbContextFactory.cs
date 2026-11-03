using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Lecture11.Infrastructure;

// EF tools use this factory without starting the console application.
public sealed class ExpenseDbContextFactory : IDesignTimeDbContextFactory<ExpenseDbContext>
{
    public ExpenseDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ExpenseDbContext>()
            .UseSqlite(DatabaseConnection.CreateSqliteConnectionString())
            .Options;

        return new ExpenseDbContext(options);
    }
}
