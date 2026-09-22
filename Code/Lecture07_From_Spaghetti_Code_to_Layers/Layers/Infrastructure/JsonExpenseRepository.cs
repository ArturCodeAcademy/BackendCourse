using System.Text.Json;
using Lecture07.Application;
using Lecture07.Domain;

namespace Lecture07.Infrastructure;

public class JsonExpenseRepository : IExpenseRepository
{
    private readonly string filePath;
    private readonly List<Expense> expenses;

    public JsonExpenseRepository(string filePath)
    {
        this.filePath = filePath;
        expenses = Load();
    }

    public List<Expense> GetAll()
    {
        return expenses;
    }

    public void Add(Expense expense)
    {
        expenses.Add(expense);
        Save();
    }

    public bool DeleteById(int id)
    {
        Expense expense = expenses.FirstOrDefault(item => item.Id == id);
        if (expense == null)
        {
            return false;
        }

        expenses.Remove(expense);
        Save();
        return true;
    }

    private List<Expense> Load()
    {
        try
        {
            if (!File.Exists(filePath)) return new List<Expense>();
            string json = File.ReadAllText(filePath);
            List<Expense> loaded = JsonSerializer.Deserialize<List<Expense>>(json);
            return loaded == null ? new List<Expense>() : loaded;
        }
        catch (JsonException)
        {
            Console.WriteLine("Invalid JSON. Starting with empty data.");
            return new List<Expense>();
        }
        catch (IOException ex)
        {
            Console.WriteLine("Could not read data: " + ex.Message);
            return new List<Expense>();
        }
    }

    private void Save()
    {
        try
        {
            string directory = Path.GetDirectoryName(filePath);
            Directory.CreateDirectory(directory);
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(filePath, JsonSerializer.Serialize(expenses, options));
        }
        catch (IOException ex)
        {
            Console.WriteLine("Could not save data: " + ex.Message);
        }
    }
}

