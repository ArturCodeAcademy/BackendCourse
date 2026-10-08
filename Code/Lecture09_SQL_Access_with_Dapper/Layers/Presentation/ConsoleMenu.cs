using System.Globalization;
using Lecture09.Application;
using Lecture09.Domain;

namespace Lecture09.Presentation;

public class ConsoleMenu
{
    private readonly IUserRepository users;
    private readonly ExpenseService expenses;
    private User? currentUser;

    public ConsoleMenu(IUserRepository users, ExpenseService expenses)
    {
        this.users = users;
        this.expenses = expenses;
    }

    public void Run()
    {
        Console.Write("Existing username: ");
        string username = Console.ReadLine()?.Trim() ?? string.Empty;
        currentUser = users.FindByUsername(username);
        if (currentUser is null)
        {
            Console.WriteLine("This focused Dapper demo expects a seeded user. Run the Project once.");
            return;
        }

        while (true)
        {
            Console.WriteLine("1 Add  2 List mine  3 Delete mine  0 Exit");
            Console.Write("Choose: ");
            switch (Console.ReadLine())
            {
                case "1": Add(); break;
                case "2": List(); break;
                case "3": Delete(); break;
                case "0": return;
                default: Console.WriteLine("Unknown command."); break;
            }
        }
    }

    private void Add()
    {
        Console.Write("Name: "); string? name = Console.ReadLine();
        Console.Write("Amount: "); string? raw = Console.ReadLine();
        Console.Write("Category: "); string? category = Console.ReadLine();
        if (!decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal amount))
        {
            Console.WriteLine("Use a decimal number such as 12.50."); return;
        }
        Console.WriteLine(expenses.AddForUser(currentUser, name, amount, category, out string message) ? message : message);
    }

    private void List()
    {
        foreach (Expense expense in expenses.GetMine(currentUser!))
            Console.WriteLine($"{expense.Id}: {expense.Name} | {expense.Amount:F2} | {expense.Category}");
    }

    private void Delete()
    {
        Console.Write("Id: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Id must be an integer."); return; }
        Console.WriteLine(expenses.DeleteMine(currentUser!, id) ? "Deleted." : "Not found in your account.");
    }
}
