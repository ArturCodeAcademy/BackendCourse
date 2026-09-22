using System.Globalization;
using System.Text.Json;

Console.WriteLine("Expense Tracker - Interface Version");
Console.WriteLine("===================================");
Console.WriteLine("1. Temporary memory storage");
Console.WriteLine("2. JSON file storage");
Console.Write("Choose storage: ");
string storageChoice = Console.ReadLine();

IExpenseRepository repository;
if (storageChoice == "1")
{
    repository = new InMemoryExpenseRepository();
    Console.WriteLine("Using temporary memory storage.");
}
else
{
    string dataPath = Path.Combine(Directory.GetCurrentDirectory(), "data", "expenses.json");
    repository = new JsonExpenseRepository(dataPath);
    Console.WriteLine("Using JSON file storage: " + dataPath);
}

ExpenseTrackerApp app = new ExpenseTrackerApp(repository);
app.Run();

public interface IExpenseRepository
{
    List<Expense> GetAll();
    void Add(Expense expense);
    bool DeleteById(int id);
}

public class InMemoryExpenseRepository : IExpenseRepository
{
    private readonly List<Expense> expenses = new List<Expense>();

    public List<Expense> GetAll()
    {
        return expenses;
    }

    public void Add(Expense expense)
    {
        expenses.Add(expense);
    }

    public bool DeleteById(int id)
    {
        Expense expense = expenses.FirstOrDefault(item => item.Id == id);
        if (expense == null) return false;
        expenses.Remove(expense);
        return true;
    }
}

public class JsonExpenseRepository : IExpenseRepository
{
    private readonly string filePath;
    private readonly List<Expense> expenses;

    public JsonExpenseRepository(string path)
    {
        filePath = path;
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
        if (expense == null) return false;
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
            Console.WriteLine("Invalid JSON. Starting with an empty list.");
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

public class ExpenseTrackerApp
{
    private readonly IExpenseRepository repository;
    private bool isRunning = true;

    public ExpenseTrackerApp(IExpenseRepository repository)
    {
        this.repository = repository;
    }

    public void Run()
    {
        while (isRunning)
        {
            PrintMenu();
            string command = Console.ReadLine();
            Console.WriteLine();

            switch (command)
            {
                case "1": AddExpense(); break;
                case "2": ListExpenses(); break;
                case "3": FilterByCategory(); break;
                case "4": DeleteExpense(); break;
                case "5": ShowSummary(); break;
                case "6": isRunning = false; Console.WriteLine("Goodbye."); break;
                default: Console.WriteLine("Choose a number from 1 to 6."); break;
            }
        }
    }

    private void PrintMenu()
    {
        Console.WriteLine();
        Console.WriteLine("1. Add expense");
        Console.WriteLine("2. List expenses");
        Console.WriteLine("3. Filter by category");
        Console.WriteLine("4. Delete expense");
        Console.WriteLine("5. Show summary");
        Console.WriteLine("6. Exit");
        Console.Write("Choose command: ");
    }

    private void AddExpense()
    {
        Console.Write("Name: ");
        string name = Console.ReadLine();
        Console.Write("Amount: ");
        string amountText = Console.ReadLine();
        Console.Write("Category: ");
        string category = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(category))
        {
            Console.WriteLine("Name and category cannot be empty.");
            return;
        }
        if (!decimal.TryParse(amountText, CultureInfo.InvariantCulture, out decimal amount) || amount <= 0)
        {
            Console.WriteLine("Amount must be a positive number, for example 4.50.");
            return;
        }

        List<Expense> expenses = repository.GetAll();
        int nextId = expenses.Count == 0 ? 1 : expenses.Max(expense => expense.Id) + 1;
        repository.Add(new Expense(nextId, name, amount, category, DateTime.Now));
        Console.WriteLine("Expense added.");
    }

    private void ListExpenses()
    {
        List<Expense> expenses = repository.GetAll().OrderBy(expense => expense.Id).ToList();
        if (expenses.Count == 0) { Console.WriteLine("No expenses yet."); return; }
        foreach (Expense expense in expenses) Console.WriteLine(expense);
    }

    private void FilterByCategory()
    {
        Console.Write("Category: ");
        string category = Console.ReadLine();
        List<Expense> results = repository.GetAll()
            .Where(expense => expense.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
            .ToList();
        if (results.Count == 0) { Console.WriteLine("No matching expenses."); return; }
        foreach (Expense expense in results) Console.WriteLine(expense);
    }

    private void DeleteExpense()
    {
        Console.Write("Id to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Enter a valid id."); return; }
        Console.WriteLine(repository.DeleteById(id) ? "Expense deleted." : "No expense with that id.");
    }

    private void ShowSummary()
    {
        List<Expense> expenses = repository.GetAll();
        Console.WriteLine("Count: " + expenses.Count);
        Console.WriteLine("Total: " + expenses.Sum(expense => expense.Amount).ToString("F2", CultureInfo.InvariantCulture) + " EUR");
    }
}

public class Expense
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public string Category { get; set; }
    public DateTime CreatedAt { get; set; }

    public Expense() { }

    public Expense(int id, string name, decimal amount, string category, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Amount = amount;
        Category = category;
        CreatedAt = createdAt;
    }

    public override string ToString()
    {
        return Id + ". " + Name + " - " + Amount.ToString("F2", CultureInfo.InvariantCulture) + " EUR - " + Category;
    }
}
