using Dapper;
using Lecture09.Application;
using Lecture09.Domain;

namespace Lecture09.Infrastructure;

public class DapperUserRepository : IUserRepository
{
    private readonly SqliteDatabase database;
    public DapperUserRepository(SqliteDatabase database) => this.database = database;

    public User? FindByUsername(string username)
    {
        using var connection = database.OpenConnection();
        return connection.QuerySingleOrDefault<User>(
            "SELECT Id, Username, PasswordHash FROM Users WHERE Username = @Username;",
            new { Username = username });
    }

    public User? FindById(int id)
    {
        using var connection = database.OpenConnection();
        return connection.QuerySingleOrDefault<User>(
            "SELECT Id, Username, PasswordHash FROM Users WHERE Id = @Id;",
            new { Id = id });
    }

    public int Add(User user)
    {
        using var connection = database.OpenConnection();
        return connection.ExecuteScalar<int>("""
            INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash);
            SELECT last_insert_rowid();
            """, user);
    }
}
