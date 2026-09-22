using System.Globalization;
using System.Text.Json;

string dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "data");
string dataFilePath = Path.Combine(dataDirectory, "expenses.json");
List<Expense> expenses = LoadExpenses(dataFilePath);
bool isRunning = true;

Console.WriteLine("Personal Expense Tracker with Classes");
Console.WriteLine("=====================================");
Console.WriteLine("Data file: " + dataFilePath);

while (isRunning)
{
    Console.WriteLine();
    PrintMenu();
    string command = Console.ReadLine();
    Console.WriteLine();

    switch (command)
    {
        case "1":
            AddExpense(expenses);
            SaveExpenses(dataFilePath, expenses);
            break;
        case "2":
            ListExpenses(expenses);
            break;
        case "3":
            SearchExpenses(expenses);
            break;
        case "4":
            FilterByCategory(expenses);
            break;
        case "5":
            DeleteExpense(expenses);
            SaveExpenses(dataFilePath, expenses);
            break;
        case "6":
            ShowSummary(expenses);
            break;
        case "7":
            SaveExpenses(dataFilePath, expenses);
            Console.WriteLine("Saved manually.");
            break;
        case "8":
            isRunning = false;
            SaveExpenses(dataFilePath, expenses);
            Console.WriteLine("Saved. Goodbye.");
            break;
        default:
            Console.WriteLine("Unknown command. Choose a number from 1 to 8.");
            break;
    }
}

void PrintMenu()
{
    Console.WriteLine("1. Add expense");
    Console.WriteLine("2. List expenses");
    Console.WriteLine("3. Search by name");
    Console.WriteLine("4. Filter by category");
    Console.WriteLine("5. Delete expense");
    Console.WriteLine("6. Show summary");
    Console.WriteLine("7. Save");
    Console.WriteLine("8. Exit");
    Console.Write("Choose command: ");
}

List<Expense> LoadExpenses(string path)
{
    try
    {
        if (!File.Exists(path))
        {
            return new List<Expense>();
        }

        string json = File.ReadAllText(path);
        List<Expense> loadedExpenses = JsonSerializer.Deserialize<List<Expense>>(json);
        return loadedExpenses ?? new List<Expense>();
    }
    catch (JsonException)
    {
        Console.WriteLine("The JSON file is invalid. Starting with an empty list.");
        return new List<Expense>();
    }
    catch (IOException ex)
    {
        Console.WriteLine("Could not read the data file: " + ex.Message);
        return new List<Expense>();
    }
}

void SaveExpenses(string path, List<Expense> items)
{
    try
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(path, JsonSerializer.Serialize(items, options));
    }
    catch (IOException ex)
    {
        Console.WriteLine("Could not save the data file: " + ex.Message);
    }
}

void AddExpense(List<Expense> items)
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
        Console.WriteLine("Amount must be a positive number. Use dot, for example 4.50.");
        return;
    }

    int nextId = items.Count == 0 ? 1 : items.Max(expense => expense.Id) + 1;
    Expense expense = new Expense(nextId, name, amount, category, DateTime.Now);
    items.Add(expense);
    Console.WriteLine("Expense added: " + expense);
}

void ListExpenses(List<Expense> items)
{
    if (items.Count == 0)
    {
        Console.WriteLine("No expenses yet.");
        return;
    }

    foreach (Expense expense in items.OrderBy(expense => expense.Name))
    {
        Console.WriteLine(expense);
    }
}

void SearchExpenses(List<Expense> items)
{
    Console.Write("Search text: ");
    string searchText = Console.ReadLine();
    List<Expense> results = items
        .Where(expense => expense.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
        .OrderBy(expense => expense.Name)
        .ToList();
    PrintResults(results);
}

void FilterByCategory(List<Expense> items)
{
    Console.Write("Category: ");
    string category = Console.ReadLine();
    List<Expense> results = items
        .Where(expense => expense.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
        .OrderBy(expense => expense.Name)
        .ToList();
    PrintResults(results);
}

void DeleteExpense(List<Expense> items)
{
    ListExpenses(items);
    if (items.Count == 0) return;

    Console.Write("Id to delete: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Enter a valid id.");
        return;
    }

    Expense expense = items.FirstOrDefault(item => item.Id == id);
    if (expense == null)
    {
        Console.WriteLine("No expense with that id.");
        return;
    }

    items.Remove(expense);
    Console.WriteLine("Deleted: " + expense.Name);
}

void ShowSummary(List<Expense> items)
{
    decimal total = items.Sum(expense => expense.Amount);
    Console.WriteLine("Expense count: " + items.Count);
    Console.WriteLine("Total: " + total.ToString("F2", CultureInfo.InvariantCulture) + " EUR");

    foreach (IGrouping<string, Expense> group in items.GroupBy(expense => expense.Category).OrderBy(group => group.Key))
    {
        Console.WriteLine(group.Key + ": " + group.Sum(expense => expense.Amount).ToString("F2", CultureInfo.InvariantCulture) + " EUR");
    }
}

void PrintResults(List<Expense> results)
{
    if (results.Count == 0)
    {
        Console.WriteLine("No matching expenses found.");
        return;
    }

    foreach (Expense expense in results)
    {
        Console.WriteLine(expense);
    }
}

public class Expense
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public string Category { get; set; }
    public DateTime CreatedAt { get; set; }

    public Expense()
    {
    }

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
        return Id + ". " + Name + " - " + Amount.ToString("F2", CultureInfo.InvariantCulture) + " EUR - " + Category + " - " + CreatedAt.ToString("yyyy-MM-dd HH:mm");
    }
}
