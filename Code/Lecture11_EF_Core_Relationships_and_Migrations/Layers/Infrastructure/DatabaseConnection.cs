using Lecture11.Domain;
using Microsoft.EntityFrameworkCore;

namespace Lecture11.Infrastructure;

public static class DatabaseConnection
{
    public static string CreateSqliteConnectionString()
    {
        string folder = Path.Combine(AppContext.BaseDirectory, "data");
        Directory.CreateDirectory(folder);
        string databasePath = Path.Combine(folder, "relationships.db");

        return $"Data Source={databasePath}";
    }
}
