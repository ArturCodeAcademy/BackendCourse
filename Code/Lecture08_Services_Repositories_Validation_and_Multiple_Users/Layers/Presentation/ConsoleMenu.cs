using System.Globalization;
using Lecture08.Application;
using Lecture08.Domain;

namespace Lecture08.Presentation;

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
                if (!RunGuestMenu())
                {
                    return;
                }
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
            case "1":
                Register();
                return true;
            case "2":
                Login();
                return true;
            case "0":
                return false;
            default:
                Console.WriteLine("Unknown command.");
                return true;
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
            case "1":
                AddExpense();
                return true;
            case "2":
                ListExpenses();
                return true;
            case "3":
                DeleteExpense();
                return true;
            case "4":
                ShowTotal();
                return true;
            case "5":
                Console.WriteLine("Signed out.");
                currentUser = null;
                return true;
            case "0":
                return false;
            default:
                Console.WriteLine("Unknown command.");
                return true;
        }
    }

    private void Register()
    {
        Console.Write("Username: ");
        string? username = Console.ReadLine();
        Console.Write("Password (visible in this learning console): ");
        string? password = Console.ReadLine();

        Console.WriteLine(authService.Register(username, password, out string message)
            ? message
            : "Registration failed: " + message);
    }

    private void Login()
    {
        Console.Write("Username: ");
        string? username = Console.ReadLine();
        Console.Write("Password (visible in this learning console): ");
        string? password = Console.ReadLine();

        currentUser = authService.Login(username, password, out string message);
        Console.WriteLine(message);
    }

    private void AddExpense()
    {
        Console.Write("Name: ");
        string? name = Console.ReadLine();
        Console.Write("Amount: ");
        string? rawAmount = Console.ReadLine();
        Console.Write("Category: ");
        string? category = Console.ReadLine();

        bool isAmountValid = decimal.TryParse(
            rawAmount,
            NumberStyles.Number,
            CultureInfo.InvariantCulture,
            out decimal amount);

        if (!isAmountValid)
        {
            Console.WriteLine("Amount must be a number, for example 12.50.");
            return;
        }

        Console.WriteLine(expenseService.AddForUser(currentUser, name, amount, category, out string message)
            ? message
            : "Expense was not saved: " + message);
    }

    private void ListExpenses()
    {
        List<Expense> myExpenses = expenseService.GetForUser(currentUser!);

        if (myExpenses.Count == 0)
        {
            Console.WriteLine("You have no expenses yet.");
            return;
        }

        foreach (Expense expense in myExpenses)
        {
            Console.WriteLine(expense);
        }
    }

    private void DeleteExpense()
    {
        Console.Write("Expense id: ");

        if (!int.TryParse(Console.ReadLine(), out int expenseId))
        {
            Console.WriteLine("Id must be an integer.");
            return;
        }

        Console.WriteLine(expenseService.DeleteForUser(currentUser, expenseId, out string message)
            ? message
            : "Deletion failed: " + message);
    }

    private void ShowTotal()
    {
        decimal total = expenseService.GetTotalForUser(currentUser!);
        Console.WriteLine("Your total: " + total.ToString("F2", CultureInfo.InvariantCulture));
    }
}
