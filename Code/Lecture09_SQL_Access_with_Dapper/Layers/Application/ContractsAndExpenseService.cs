using Lecture09.Domain;

namespace Lecture09.Application;

public interface IUserRepository
{
    User? FindByUsername(string username);
    User? FindById(int id);
    int Add(User user);
}

public interface IExpenseRepository
{
    IReadOnlyList<Expense> GetForUser(int userId);
    int Add(Expense expense);
    bool DeleteForUser(int expenseId, int userId);
}

public class ExpenseService
{
    private readonly IExpenseRepository expenses;

    public ExpenseService(IExpenseRepository expenses) => this.expenses = expenses;

    public bool AddForUser(User? currentUser, string? name, decimal amount, string? category, out string message)
    {
        if (currentUser is null) { message = "Sign in first."; return false; }
        name = name?.Trim() ?? string.Empty;
        category = category?.Trim() ?? string.Empty;
        if (name.Length < 2) { message = "Name needs at least 2 characters."; return false; }
        if (amount <= 0) { message = "Amount must be greater than zero."; return false; }
        if (category.Length == 0) { message = "Category is required."; return false; }

        expenses.Add(new Expense { UserId = currentUser.Id, Name = name, Amount = amount, Category = category });
        message = "Expense saved in SQLite.";
        return true;
    }

    public IReadOnlyList<Expense> GetMine(User currentUser) => expenses.GetForUser(currentUser.Id);

    public bool DeleteMine(User currentUser, int expenseId) =>
        expenses.DeleteForUser(expenseId, currentUser.Id);
}
