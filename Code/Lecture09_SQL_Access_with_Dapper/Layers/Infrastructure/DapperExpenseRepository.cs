using Dapper;
using Lecture09.Application;
using Lecture09.Domain;

namespace Lecture09.Infrastructure;

public class DapperExpenseRepository : IExpenseRepository
{
    private readonly SqliteDatabase database;
    public DapperExpenseRepository(SqliteDatabase database) => this.database = database;

    public IReadOnlyList<Expense> GetForUser(int userId)
    {
        using var connection = database.OpenConnection();
        return connection.Query<Expense>(
            "SELECT Id, UserId, Name, Amount, Category, CreatedAt FROM Expenses WHERE UserId = @UserId ORDER BY Id DESC;",
            new { UserId = userId }).AsList();
    }

    public int Add(Expense expense)
    {
        using var connection = database.OpenConnection();
        return connection.ExecuteScalar<int>("""
            INSERT INTO Expenses (UserId, Name, Amount, Category, CreatedAt)
            VALUES (@UserId, @Name, @Amount, @Category, @CreatedAt);
            SELECT last_insert_rowid();
            """, expense);
    }

    public bool DeleteForUser(int expenseId, int userId)
    {
        using var connection = database.OpenConnection();
        int rows = connection.Execute(
            "DELETE FROM Expenses WHERE Id = @ExpenseId AND UserId = @UserId;",
            new { ExpenseId = expenseId, UserId = userId });
        return rows == 1;
    }
}
