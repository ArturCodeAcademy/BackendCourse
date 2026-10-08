using Dapper;
using Microsoft.Data.Sqlite;

namespace Lecture09.Infrastructure;

public class SqliteDatabase
{
    private readonly string connectionString;

    public SqliteDatabase(string databaseFile)
    {
        connectionString = new SqliteConnectionStringBuilder { DataSource = databaseFile }.ToString();
    }

    public SqliteConnection OpenConnection()
    {
        var connection = new SqliteConnection(connectionString);
        connection.Open();
        return connection;
    }

    public void Initialize()
    {
        using SqliteConnection connection = OpenConnection();
        connection.Execute("""
            PRAGMA foreign_keys = ON;
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL COLLATE NOCASE UNIQUE,
                PasswordHash TEXT NOT NULL
            );
            CREATE TABLE IF NOT EXISTS Expenses (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                UserId INTEGER NOT NULL,
                Name TEXT NOT NULL,
                Amount NUMERIC NOT NULL CHECK (Amount > 0),
                Category TEXT NOT NULL,
                CreatedAt TEXT NOT NULL,
                FOREIGN KEY (UserId) REFERENCES Users(Id)
            );
            """);
    }
}
