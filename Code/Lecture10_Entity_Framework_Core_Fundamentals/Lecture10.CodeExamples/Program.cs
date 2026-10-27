using Microsoft.Data.Sqlite;

// A connection string describes where SQLite stores the database file.
const string connectionString = "Data Source=library-demo.db";

await using var connection = new SqliteConnection(connectionString);
await connection.OpenAsync();

await CreateTableAsync(connection);
await ClearTableAsync(connection);
await InsertBookAsync(connection, "The Pragmatic Programmer", "Andrew Hunt", 1999);
await InsertBookAsync(connection, "Clean Code", "Robert C. Martin", 2008);

Console.WriteLine("After INSERT:");
await PrintBooksAsync(connection);

await UpdatePublicationYearAsync(connection, "Clean Code", 2009);

Console.WriteLine("\nAfter UPDATE:");
await PrintBooksAsync(connection);

await DeleteBookAsync(connection, "The Pragmatic Programmer");

Console.WriteLine("\nAfter DELETE:");
await PrintBooksAsync(connection);

static async Task CreateTableAsync(SqliteConnection connection)
{
    await using SqliteCommand command = connection.CreateCommand();
    command.CommandText = """
        CREATE TABLE IF NOT EXISTS Books (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Title TEXT NOT NULL,
            Author TEXT NOT NULL,
            PublishedYear INTEGER NOT NULL
        );
        """;

    await command.ExecuteNonQueryAsync();
}

static async Task ClearTableAsync(SqliteConnection connection)
{
    await using SqliteCommand command = connection.CreateCommand();
    command.CommandText = "DELETE FROM Books;";
    await command.ExecuteNonQueryAsync();
}

static async Task InsertBookAsync(
    SqliteConnection connection,
    string title,
    string author,
    int publishedYear)
{
    await using SqliteCommand command = connection.CreateCommand();
    command.CommandText = """
        INSERT INTO Books (Title, Author, PublishedYear)
        VALUES ($title, $author, $publishedYear);
        """;

    // Parameters keep data separate from SQL text.
    command.Parameters.AddWithValue("$title", title);
    command.Parameters.AddWithValue("$author", author);
    command.Parameters.AddWithValue("$publishedYear", publishedYear);

    await command.ExecuteNonQueryAsync();
}

static async Task PrintBooksAsync(SqliteConnection connection)
{
    await using SqliteCommand command = connection.CreateCommand();
    command.CommandText = """
        SELECT Id, Title, Author, PublishedYear
        FROM Books
        ORDER BY PublishedYear;
        """;

    await using SqliteDataReader reader = await command.ExecuteReaderAsync();
    while (await reader.ReadAsync())
    {
        int id = reader.GetInt32(0);
        string title = reader.GetString(1);
        string author = reader.GetString(2);
        int publishedYear = reader.GetInt32(3);

        Console.WriteLine($"{id}: {title} by {author} ({publishedYear})");
    }
}

static async Task UpdatePublicationYearAsync(
    SqliteConnection connection,
    string title,
    int publishedYear)
{
    await using SqliteCommand command = connection.CreateCommand();
    command.CommandText = """
        UPDATE Books
        SET PublishedYear = $publishedYear
        WHERE Title = $title;
        """;

    command.Parameters.AddWithValue("$title", title);
    command.Parameters.AddWithValue("$publishedYear", publishedYear);

    await command.ExecuteNonQueryAsync();
}

static async Task DeleteBookAsync(SqliteConnection connection, string title)
{
    await using SqliteCommand command = connection.CreateCommand();
    command.CommandText = "DELETE FROM Books WHERE Title = $title;";
    command.Parameters.AddWithValue("$title", title);
    await command.ExecuteNonQueryAsync();
}
