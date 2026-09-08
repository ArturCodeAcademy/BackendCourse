using System.Globalization;
using System.Text.Json;

string dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "data");
string dataFilePath = Path.Combine(dataDirectory, "expenses.json");

List<Dictionary<string, string>> expenses = LoadExpenses(dataFilePath);
bool isRunning = true;

Console.WriteLine("Personal Expense Tracker with JSON");
Console.WriteLine("==================================");
Console.WriteLine("Data file: " + dataFilePath);

while (isRunning)
{
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
    Console.WriteLine();
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

List<Dictionary<string, string>> LoadExpenses(string path)
{
    try
    {
        if (!File.Exists(path))
        {
            return new List<Dictionary<string, string>>();
        }

        string json = File.ReadAllText(path);
        List<Dictionary<string, string>> loadedExpenses = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);

        if (loadedExpenses == null)
        {
            return new List<Dictionary<string, string>>();
        }

        return loadedExpenses;
    }
    catch (JsonException)
    {
        Console.WriteLine("The JSON file is invalid. Starting with an empty list.");
        return new List<Dictionary<string, string>>();
    }
    catch (IOException ex)
    {
        Console.WriteLine("Could not read the data file: " + ex.Message);
        return new List<Dictionary<string, string>>();
    }
}

void SaveExpenses(string path, List<Dictionary<string, string>> items)
{
    try
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path));

        JsonSerializerOptions options = new JsonSerializerOptions();
        options.WriteIndented = true;

        string json = JsonSerializer.Serialize(items, options);
        File.WriteAllText(path, json);
    }
    catch (IOException ex)
    {
        Console.WriteLine("Could not save the data file: " + ex.Message);
    }
    catch (UnauthorizedAccessException ex)
    {
        Console.WriteLine("No permission to save the data file: " + ex.Message);
    }
}

void AddExpense(List<Dictionary<string, string>> items)
{
    Console.Write("Name: ");
    string name = Console.ReadLine();

    Console.Write("Amount: ");
    string amountText = Console.ReadLine();

    Console.Write("Category: ");
    string category = Console.ReadLine();

    if (name == "")
    {
        Console.WriteLine("Name cannot be empty.");
        return;
    }

    if (!decimal.TryParse(amountText, CultureInfo.InvariantCulture, out decimal amount))
    {
        Console.WriteLine("Amount must be a number. Use dot, for example 4.50.");
        return;
    }

    if (amount <= 0)
    {
        Console.WriteLine("Amount must be greater than zero.");
        return;
    }

    if (category == "")
    {
        Console.WriteLine("Category cannot be empty.");
        return;
    }

    Dictionary<string, string> expense = new Dictionary<string, string>();
    expense["name"] = name;
    expense["amount"] = amount.ToString("F2", CultureInfo.InvariantCulture);
    expense["category"] = category;
    expense["createdAt"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

    items.Add(expense);
    Console.WriteLine("Expense added and saved.");
}

void ListExpenses(List<Dictionary<string, string>> items)
{
    if (items.Count == 0)
    {
        Console.WriteLine("No expenses yet.");
        return;
    }

    List<Dictionary<string, string>> orderedItems = items
        .OrderBy(expense => expense["name"])
        .ToList();

    for (int index = 0; index < orderedItems.Count; index++)
    {
        PrintExpense(index + 1, orderedItems[index]);
    }
}

void SearchExpenses(List<Dictionary<string, string>> items)
{
    Console.Write("Search text: ");
    string searchText = Console.ReadLine();

    List<Dictionary<string, string>> results = items
        .Where(expense => expense["name"].ToLowerInvariant().Contains(searchText.ToLowerInvariant()))
        .ToList();

    PrintResults(results);
}

void FilterByCategory(List<Dictionary<string, string>> items)
{
    Console.Write("Category: ");
    string category = Console.ReadLine();

    List<Dictionary<string, string>> results = items
        .Where(expense => expense["category"].ToLowerInvariant() == category.ToLowerInvariant())
        .OrderBy(expense => expense["name"])
        .ToList();

    PrintResults(results);
}

void DeleteExpense(List<Dictionary<string, string>> items)
{
    if (items.Count == 0)
    {
        Console.WriteLine("No expenses to delete.");
        return;
    }

    for (int index = 0; index < items.Count; index++)
    {
        PrintExpense(index + 1, items[index]);
    }

    Console.Write("Number to delete: ");
    string numberText = Console.ReadLine();

    if (!int.TryParse(numberText, CultureInfo.InvariantCulture, out int displayNumber))
    {
        Console.WriteLine("Enter a valid number.");
        return;
    }

    int indexToDelete = displayNumber - 1;

    if (indexToDelete < 0 || indexToDelete >= items.Count)
    {
        Console.WriteLine("No expense with that number.");
        return;
    }

    string deletedName = items[indexToDelete]["name"];
    items.RemoveAt(indexToDelete);
    Console.WriteLine("Deleted: " + deletedName);
}

void ShowSummary(List<Dictionary<string, string>> items)
{
    decimal total = items
        .Select(expense => decimal.Parse(expense["amount"], CultureInfo.InvariantCulture))
        .Sum();

    bool hasFood = items.Any(expense => expense["category"].ToLowerInvariant() == "food");

    Console.WriteLine("Expense count: " + items.Count);
    Console.WriteLine("Total: " + total.ToString("F2", CultureInfo.InvariantCulture) + " EUR");
    Console.WriteLine("Has food expenses: " + hasFood);
}

void PrintResults(List<Dictionary<string, string>> results)
{
    if (results.Count == 0)
    {
        Console.WriteLine("No matching expenses found.");
        return;
    }

    for (int index = 0; index < results.Count; index++)
    {
        PrintExpense(index + 1, results[index]);
    }
}

void PrintExpense(int number, Dictionary<string, string> expense)
{
    Console.WriteLine(number + ". " + expense["name"] + " - " + expense["amount"] + " EUR - " + expense["category"] + " - " + expense["createdAt"]);
}
