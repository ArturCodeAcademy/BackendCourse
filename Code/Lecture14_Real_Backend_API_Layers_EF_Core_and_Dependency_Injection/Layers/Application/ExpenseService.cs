using Lecture14.Domain;

namespace Lecture14.Application;

public sealed class ExpenseService
{
    private readonly IExpenseRepository repository;

    public ExpenseService(IExpenseRepository repository)
    {
        this.repository = repository;
    }

    public Task<List<Expense>> GetMineAsync(int userId, CancellationToken cancellationToken)
    {
        return repository.GetForUserAsync(userId, cancellationToken);
    }

    public Task<Expense?> GetMineByIdAsync(int userId, int expenseId, CancellationToken cancellationToken)
    {
        return repository.GetByIdForUserAsync(userId, expenseId, cancellationToken);
    }

    public Task<Expense> CreateAsync(
        int userId,
        string? name,
        decimal amount,
        string? category,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.");
        }

        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            throw new ArgumentException("Category is required.");
        }

        var expense = new Expense
        {
            UserId = userId,
            Name = name.Trim(),
            Amount = amount,
            Category = category.Trim()
        };

        return repository.AddAsync(expense, cancellationToken);
    }
}