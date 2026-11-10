using Lecture13.Domain;

namespace Lecture13.Application;

public sealed class ExpenseService
{
    private readonly IExpenseRepository repository;

    public ExpenseService(IExpenseRepository repository)
    {
        this.repository = repository;
    }

    public IReadOnlyList<Expense> GetForUser(int userId)
    {
        return repository.GetForUser(userId);
    }

    public Expense? Get(int userId, int expenseId)
    {
        return repository.GetByIdForUser(userId, expenseId);
    }

    public Expense Create(int userId, string? name, decimal amount, string? category)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("name", "Name is required.");
        }

        if (amount <= 0)
        {
            throw new ValidationException("amount", "Amount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            throw new ValidationException("category", "Category is required.");
        }

        var expense = new Expense
        {
            UserId = userId,
            Name = name.Trim(),
            Amount = amount,
            Category = category.Trim()
        };

        return repository.Add(expense);
    }
}