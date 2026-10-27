using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

// Both sections use the same SQLite file and the same Books table.
const string connectionString = "Data Source=library-demo.db";

Console.WriteLine("=== Part 1: raw SQL with Microsoft.Data.Sqlite ===");
await using (var connection = new SqliteConnection(connectionString))
{
    await connection.OpenAsync();

    await CreateTableAsync(connection);
    await ClearTableAsync(connection);
    await InsertBookWithSqlAsync(connection, "The Pragmatic Programmer", "Andrew Hunt", 1999);
    await InsertBookWithSqlAsync(connection, "Clean Code", "Robert C. Martin", 2008);

    Console.WriteLine("SQL SELECT result:");
    await PrintBooksWithSqlAsync(connection);
}

Console.WriteLine("\n=== Part 2: EF Core and LINQ ===");
var options = new DbContextOptionsBuilder<LibraryDbContext>()
    .UseSqlite(connectionString)
    .Options;

await using (var context = new LibraryDbContext(options))
{
    // EF Core translates this LINQ query to SQL when ToListAsync executes it.
    List<Book> modernBooks = await context.Books
        .Where(book => book.PublishedYear >= 2000)
        .OrderBy(book => book.Title)
        .ToListAsync();

    Console.WriteLine("LINQ WHERE + ORDER BY result:");
    foreach (Book book in modernBooks)
    {
        Console.WriteLine($"{book.Id}: {book.Title} by {book.Author} ({book.PublishedYear})");
    }

    // EF tracks the entity. SaveChangesAsync translates this modification to UPDATE.
    Book? cleanCode = await context.Books
        .SingleOrDefaultAsync(book => book.Title == "Clean Code");

    if (cleanCode is not null)
    {
        cleanCode.PublishedYear = 2009;
        await context.SaveChangesAsync();
    }

    // Add changes the entity state to Added; SaveChangesAsync sends INSERT.
    context.Books.Add(new Book
    {
        Title = "Refactoring",
        Author = "Martin Fowler",
        PublishedYear = 1999
    });
    await context.SaveChangesAsync();

    // Remove marks an entity as Deleted; SaveChangesAsync sends DELETE.
    Book? pragmaticProgrammer = await context.Books
        .SingleOrDefaultAsync(book => book.Title == "The Pragmatic Programmer");

    if (pragmaticProgrammer is not null)
    {
        context.Books.Remove(pragmaticProgrammer);
        await context.SaveChangesAsync();
    }

    List<Book> finalBooks = await context.Books
        .AsNoTracking()
        .OrderBy(book => book.Id)
        .ToListAsync();

    Console.WriteLine("\nFinal LINQ result after UPDATE, INSERT and DELETE:");
    foreach (Book book in finalBooks)
    {
        Console.WriteLine($"{book.Id}: {book.Title} by {book.Author} ({book.PublishedYear})");
    }
}

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

static async Task InsertBookWithSqlAsync(
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

    // Values are parameters, not concatenated into the SQL string.
    command.Parameters.AddWithValue("$title", title);
    command.Parameters.AddWithValue("$author", author);
    command.Parameters.AddWithValue("$publishedYear", publishedYear);

    await command.ExecuteNonQueryAsync();
}

static async Task PrintBooksWithSqlAsync(SqliteConnection connection)
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
        Console.WriteLine(
            $"{reader.GetInt32(0)}: {reader.GetString(1)} by {reader.GetString(2)} ({reader.GetInt32(3)})");
    }
}

public sealed class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int PublishedYear { get; set; }
}

public sealed class LibraryDbContext : DbContext
{
    public DbSet<Book> Books => Set<Book>();

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }
}
