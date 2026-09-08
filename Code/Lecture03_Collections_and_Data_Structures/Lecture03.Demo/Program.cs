using System.Globalization;

List<string> expenseNames = new List<string>();
List<decimal> expenseAmounts = new List<decimal>();
List<string> expenseCategories = new List<string>();

bool isRunning = true;

Console.WriteLine("Personal Expense List");
Console.WriteLine("=====================");

while (isRunning)
{
    PrintMenu();
    string command = Console.ReadLine();
    Console.WriteLine();

    switch (command)
    {
        case "1":
            AddExpense(expenseNames, expenseAmounts, expenseCategories);
            break;
        case "2":
            ListExpenses(expenseNames, expenseAmounts, expenseCategories);
            break;
        case "3":
            FindExpense(expenseNames, expenseAmounts, expenseCategories);
            break;
        case "4":
            DeleteExpense(expenseNames, expenseAmounts, expenseCategories);
            break;
        case "5":
            ShowSummary(expenseAmounts);
            break;
        case "6":
            isRunning = false;
            Console.WriteLine("Goodbye.");
            break;
        default:
            Console.WriteLine("Unknown command. Choose a number from 1 to 6.");
            break;
    }
}

void PrintMenu()
{
    Console.WriteLine();
    Console.WriteLine("1. Add expense");
    Console.WriteLine("2. List expenses");
    Console.WriteLine("3. Find expense by name");
    Console.WriteLine("4. Delete expense by number");
    Console.WriteLine("5. Show total");
    Console.WriteLine("6. Exit");
    Console.Write("Choose command: ");
}

void AddExpense(List<string> names, List<decimal> amounts, List<string> categories)
{
    Console.Write("Expense name: ");
    string name = Console.ReadLine();

    Console.Write("Amount: ");
    string amountText = Console.ReadLine();

    Console.Write("Category: ");
    string category = Console.ReadLine();

    bool amountIsValid = decimal.TryParse(
        amountText,
        CultureInfo.InvariantCulture,
        out decimal amount);

    if (name == "")
    {
        Console.WriteLine("Name cannot be empty.");
    }
    else if (!amountIsValid)
    {
        Console.WriteLine("Amount must be a number. Use dot, for example 4.50.");
    }
    else if (amount <= 0)
    {
        Console.WriteLine("Amount must be greater than zero.");
    }
    else if (category == "")
    {
        Console.WriteLine("Category cannot be empty.");
    }
    else
    {
        names.Add(name);
        amounts.Add(amount);
        categories.Add(category);
        Console.WriteLine("Expense added.");
    }
}

void ListExpenses(List<string> names, List<decimal> amounts, List<string> categories)
{
    if (names.Count == 0)
    {
        Console.WriteLine("No expenses yet.");
        return;
    }

    for (int index = 0; index < names.Count; index++)
    {
        int displayNumber = index + 1;
        Console.WriteLine(displayNumber + ". " + names[index] + " - " + amounts[index].ToString("F2", CultureInfo.InvariantCulture) + " EUR - " + categories[index]);
    }
}

void FindExpense(List<string> names, List<decimal> amounts, List<string> categories)
{
    Console.Write("Search name: ");
    string searchName = Console.ReadLine();

    bool found = false;

    for (int index = 0; index < names.Count; index++)
    {
        if (names[index].ToLowerInvariant().Contains(searchName.ToLowerInvariant()))
        {
            int displayNumber = index + 1;
            Console.WriteLine(displayNumber + ". " + names[index] + " - " + amounts[index].ToString("F2", CultureInfo.InvariantCulture) + " EUR - " + categories[index]);
            found = true;
        }
    }

    if (!found)
    {
        Console.WriteLine("No matching expenses found.");
    }
}

void DeleteExpense(List<string> names, List<decimal> amounts, List<string> categories)
{
    if (names.Count == 0)
    {
        Console.WriteLine("No expenses to delete.");
        return;
    }

    ListExpenses(names, amounts, categories);
    Console.Write("Enter expense number to delete: ");
    string numberText = Console.ReadLine();

    bool numberIsValid = int.TryParse(numberText, CultureInfo.InvariantCulture, out int displayNumber);
    int index = displayNumber - 1;

    if (!numberIsValid || index < 0 || index >= names.Count)
    {
        Console.WriteLine("Invalid expense number.");
        return;
    }

    string deletedName = names[index];
    names.RemoveAt(index);
    amounts.RemoveAt(index);
    categories.RemoveAt(index);

    Console.WriteLine("Deleted: " + deletedName);
}

void ShowSummary(List<decimal> amounts)
{
    decimal total = 0m;

    foreach (decimal amount in amounts)
    {
        total += amount;
    }

    Console.WriteLine("Expense count: " + amounts.Count);
    Console.WriteLine("Total: " + total.ToString("F2", CultureInfo.InvariantCulture) + " EUR");
}
