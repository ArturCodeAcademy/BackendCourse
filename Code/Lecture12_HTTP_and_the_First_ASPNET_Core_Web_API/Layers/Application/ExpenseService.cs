using Lecture12.Domain;

namespace Lecture12.Application;

public class ExpenseService
{
    private readonly IExpenseRepository repository;

    public ExpenseService(IExpenseRepository repository)
    {
        this.repository = repository;
    }

    public IReadOnlyList<Expense> GetAll()
    {
        return repository.GetAll();
    }

    public Expense? Get(int id)
    {
        return repository.GetById(id);
    }

    public Expense Add(string? name, decimal amount, string? category)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.");
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            throw new ArgumentException("Category is required.");
        }

        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.");
        }

        var expense = new Expense
        {
            Name = name.Trim(),
            Amount = amount,
            Category = category.Trim()
        };

        return repository.Add(expense);
    }
}