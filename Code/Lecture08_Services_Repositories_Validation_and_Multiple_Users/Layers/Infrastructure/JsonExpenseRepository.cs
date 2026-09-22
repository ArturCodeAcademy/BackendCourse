using System.Text.Json;
using Lecture08.Application;
using Lecture08.Domain;

namespace Lecture08.Infrastructure;

public class JsonExpenseRepository : IExpenseRepository
{
    private readonly string filePath;
    private readonly List<Expense> expenses;

    public JsonExpenseRepository(string filePath)
    {
        this.filePath = filePath;
        expenses = Load();
    }

    public List<Expense> GetAll() => expenses.ToList();

    public List<Expense> GetByUserId(int userId) =>
        expenses.Where(expense => expense.UserId == userId).ToList();

    public void Add(Expense expense)
    {
        expenses.Add(expense);
        Save();
    }

    public bool DeleteByIdForUser(int expenseId, int userId)
    {
        Expense? expense = expenses.FirstOrDefault(item =>
            item.Id == expenseId && item.UserId == userId);

        if (expense is null)
        {
            return false;
        }

        expenses.Remove(expense);
        Save();
        return true;
    }

    private List<Expense> Load()
    {
        if (!File.Exists(filePath))
        {
            return new List<Expense>();
        }

        try
        {
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Expense>>(json) ?? new List<Expense>();
        }
        catch (JsonException)
        {
            Console.WriteLine("Expenses file is invalid. Starting with an empty expense list.");
            return new List<Expense>();
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
        File.WriteAllText(filePath, JsonSerializer.Serialize(expenses, options));
    }
}
