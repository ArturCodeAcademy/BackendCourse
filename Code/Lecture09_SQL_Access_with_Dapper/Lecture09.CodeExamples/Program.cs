using Dapper;
using Lecture09.Infrastructure;

string file = Path.Combine(AppContext.BaseDirectory, "example.db");
var database = new SqliteDatabase(file);
database.Initialize();

using var connection = database.OpenConnection();

var newUser = new
{
    Username = "alice" + Random.Shared.Next(),
    PasswordHash = "hash-is-not-a-password"
};

int userId = connection.ExecuteScalar<int>("""
    INSERT INTO Users (Username, PasswordHash)
    VALUES (@Username, @PasswordHash);
    SELECT last_insert_rowid();
    """, newUser);

var newExpense = new
{
    UserId = userId,
    Name = "Coffee",
    Amount = 3.50m,
    Category = "Food",
    CreatedAt = DateTime.UtcNow
};

connection.Execute("""
    INSERT INTO Expenses (UserId, Name, Amount, Category, CreatedAt)
    VALUES (@UserId, @Name, @Amount, @Category, @CreatedAt);
    """, newExpense);

var rows = connection.Query("""
    SELECT Name, Amount
    FROM Expenses
    WHERE UserId = @UserId;
    """, new { UserId = userId });

foreach (var row in rows)
{
    Console.WriteLine($"{row.Name}: {row.Amount}");
}

Console.WriteLine("Dapper mapped SQL results without string-concatenating user input.");