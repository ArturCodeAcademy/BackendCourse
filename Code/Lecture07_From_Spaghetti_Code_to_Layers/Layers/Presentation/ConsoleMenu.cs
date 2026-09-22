using System.Globalization;
using Lecture07.Application;
using Lecture07.Domain;

namespace Lecture07.Presentation;

public class ConsoleMenu
{
    private readonly ExpenseService service;
    private bool isRunning = true;

    public ConsoleMenu(ExpenseService service)
    {
        this.service = service;
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
                case "2": PrintExpenses(service.GetAll()); break;
                case "3": FilterByCategory(); break;
                case "4": DeleteExpense(); break;
                case "5": PrintSummary(); break;
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

        if (!decimal.TryParse(amountText, CultureInfo.InvariantCulture, out decimal amount))
        {
            Console.WriteLine("Amount must be a number, for example 4.50.");
            return;
        }

        service.Add(name, amount, category, out string message);
        Console.WriteLine(message);
    }

    private void FilterByCategory()
    {
        Console.Write("Category: ");
        PrintExpenses(service.GetByCategory(Console.ReadLine()));
    }

    private void DeleteExpense()
    {
        Console.Write("Id to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Enter a valid id.");
            return;
        }
        Console.WriteLine(service.Delete(id) ? "Expense deleted." : "No expense with that id.");
    }

    private void PrintSummary()
    {
        Console.WriteLine("Count: " + service.GetAll().Count);
        Console.WriteLine("Total: " + service.GetTotal().ToString("F2", CultureInfo.InvariantCulture) + " EUR");
    }

    private void PrintExpenses(List<Expense> expenses)
    {
        if (expenses.Count == 0)
        {
            Console.WriteLine("No expenses found.");
            return;
        }
        foreach (Expense expense in expenses) Console.WriteLine(expense);
    }
}

