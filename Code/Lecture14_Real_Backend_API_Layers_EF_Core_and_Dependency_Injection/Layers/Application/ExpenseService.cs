using Lecture14.Domain;

namespace Lecture14.Application;

public interface IExpenseRepository
{
    Task<List<Expense>> GetForUserAsync(int userId, CancellationToken cancellationToken);
    Task<Expense?> GetByIdForUserAsync(int userId, int expenseId, CancellationToken cancellationToken);
    Task<Expense> AddAsync(Expense expense, CancellationToken cancellationToken);
}

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

    public Task<Expense> CreateAsync(int userId, string name, decimal amount, string category, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name) || amount <= 0 || string.IsNullOrWhiteSpace(category))
        {
            throw new ArgumentException("Valid name, positive amount, and category are required.");
        }

        return repository.AddAsync(new Expense
        {
            UserId = userId,
            Name = name.Trim(),
            Amount = amount,
            Category = category.Trim()
        }, cancellationToken);
    }
}
