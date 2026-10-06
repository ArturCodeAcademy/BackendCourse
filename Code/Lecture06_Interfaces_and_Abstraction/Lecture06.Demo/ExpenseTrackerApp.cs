using System.Globalization;

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
