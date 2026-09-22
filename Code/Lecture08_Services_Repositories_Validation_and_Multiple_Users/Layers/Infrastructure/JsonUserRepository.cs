using System.Text.Json;
using Lecture08.Application;
using Lecture08.Domain;

namespace Lecture08.Infrastructure;

public class JsonUserRepository : IUserRepository
{
    private readonly string filePath;
    private readonly List<User> users;

    public JsonUserRepository(string filePath)
    {
        this.filePath = filePath;
        users = Load();
    }

    public List<User> GetAll() => users.ToList();

    public User? GetByUsername(string username) =>
        users.FirstOrDefault(user =>
            string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase));

    public void Add(User user)
    {
        users.Add(user);
        Save();
    }

    private List<User> Load()
    {
        if (!File.Exists(filePath))
        {
            return new List<User>();
        }

        try
        {
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }
        catch (JsonException)
        {
            Console.WriteLine("Users file is invalid. Starting with an empty user list.");
            return new List<User>();
        }
    }

    private void Save()
    {
        string? directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(filePath, JsonSerializer.Serialize(users, options));
    }
}
