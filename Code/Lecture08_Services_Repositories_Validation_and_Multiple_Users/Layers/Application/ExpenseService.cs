using Lecture08.Domain;

namespace Lecture08.Application;

public class ExpenseService
{
    private readonly IExpenseRepository expenses;

    public ExpenseService(IExpenseRepository expenses)
    {
        this.expenses = expenses;
    }

    public bool AddForUser(User? currentUser, string? name, decimal amount, string? category, out string message)
    {
        if (currentUser is null)
        {
            message = "Please sign in first.";
            return false;
        }

        name = name?.Trim() ?? string.Empty;
        category = category?.Trim() ?? string.Empty;

        if (name.Length < 2)
        {
            message = "Expense name must contain at least 2 characters.";
            return false;
        }

        if (amount <= 0)
        {
            message = "Amount must be greater than zero.";
            return false;
        }

        if (category.Length == 0)
        {
            message = "Category is required.";
            return false;
        }

        List<Expense> allExpenses = expenses.GetAll();
        int nextId = allExpenses.Count == 0 ? 1 : allExpenses.Max(expense => expense.Id) + 1;

        expenses.Add(new Expense
        {
            Id = nextId,
            UserId = currentUser.Id,
            Name = name,
            Amount = amount,
            Category = category,
            CreatedAt = DateTime.UtcNow
        });

        message = "Expense was saved for the current user.";
        return true;
    }

    public List<Expense> GetForUser(User currentUser) =>
        expenses.GetByUserId(currentUser.Id);

    public decimal GetTotalForUser(User currentUser) =>
        GetForUser(currentUser).Sum(expense => expense.Amount);

    public bool DeleteForUser(User? currentUser, int expenseId, out string message)
    {
        if (currentUser is null)
        {
            message = "Please sign in first.";
            return false;
        }

        if (expenses.DeleteByIdForUser(expenseId, currentUser.Id))
        {
            message = "Expense was deleted.";
            return true;
        }

        // The repository checks both Id and UserId, so another user's record cannot be deleted.
        message = "Expense was not found in your account.";
        return false;
    }
}
