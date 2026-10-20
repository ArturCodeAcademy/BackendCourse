using System.Globalization;
using Lecture09.Application;
using Lecture09.Domain;

namespace Lecture09.Presentation;

public class ConsoleMenu
{
    private readonly AuthService authService;
    private readonly ExpenseService expenseService;
    private User? currentUser;

    public ConsoleMenu(AuthService authService, ExpenseService expenseService)
    {
        this.authService = authService;
        this.expenseService = expenseService;
    }

    public void Run()
    {
        while (true)
        {
            if (currentUser is null)
            {
                if (!RunGuestMenu()) return;
            }
            else if (!RunUserMenu())
            {
                return;
            }
        }
    }

    private bool RunGuestMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== Expense Tracker: guest ===");
        Console.WriteLine("1. Register");
        Console.WriteLine("2. Login");
        Console.WriteLine("0. Exit");
        Console.Write("Choose: ");

        switch (Console.ReadLine())
        {
            case "1": Register(); return true;
            case "2": Login(); return true;
            case "0": return false;
            default: Console.WriteLine("Unknown command."); return true;
        }
    }

    private bool RunUserMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== Expense Tracker: " + currentUser!.Username + " ===");
        Console.WriteLine("1. Add expense");
        Console.WriteLine("2. List my expenses");
        Console.WriteLine("3. Delete my expense");
        Console.WriteLine("4. Show my total");
        Console.WriteLine("5. Logout");
        Console.WriteLine("0. Exit");
        Console.Write("Choose: ");

        switch (Console.ReadLine())
        {
            case "1": Add(); return true;
            case "2": List(); return true;
            case "3": Delete(); return true;
            case "4": ShowTotal(); return true;
            case "5": currentUser = null; Console.WriteLine("Signed out."); return true;
            case "0": return false;
            default: Console.WriteLine("Unknown command."); return true;
        }
    }

    private void Register()
    {
        Console.Write("Username: "); string? username = Console.ReadLine();
        Console.Write("Password: "); string? password = Console.ReadLine();
        Console.WriteLine(authService.Register(username, password, out string message) ? message : "Registration failed: " + message);
    }

    private void Login()
    {
        Console.Write("Username: "); string? username = Console.ReadLine();
        Console.Write("Password: "); string? password = Console.ReadLine();
        currentUser = authService.Login(username, password, out string message);
        Console.WriteLine(message);
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

        expenseService.AddForUser(currentUser, name, amount, category, out string message);
        Console.WriteLine(message);
    }

    private void List()
    {
        IReadOnlyList<Expense> mine = expenseService.GetMine(currentUser!);
        if (mine.Count == 0) { Console.WriteLine("You have no expenses yet."); return; }
        foreach (Expense expense in mine)
            Console.WriteLine(expense.Id + ": " + expense.Name + " | " + expense.Amount.ToString("F2", CultureInfo.InvariantCulture) + " | " + expense.Category);
    }

    private void Delete()
    {
        Console.Write("Id: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Id must be an integer."); return; }
        Console.WriteLine(expenseService.DeleteMine(currentUser!, id) ? "Deleted." : "Not found in your account.");
    }

    private void ShowTotal()
    {
        decimal total = expenseService.GetMine(currentUser!).Sum(expense => expense.Amount);
        Console.WriteLine("Your total: " + total.ToString("F2", CultureInfo.InvariantCulture));
    }
}
