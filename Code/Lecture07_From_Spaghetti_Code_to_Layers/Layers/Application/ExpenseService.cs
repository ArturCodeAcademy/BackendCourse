using Lecture07.Domain;

namespace Lecture07.Application;

public class ExpenseService
{
    private readonly IExpenseRepository repository;

    public ExpenseService(IExpenseRepository repository)
    {
        this.repository = repository;
    }

    public bool Add(string name, decimal amount, string category, out string message)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            message = "Name cannot be empty.";
            return false;
        }
        if (amount <= 0)
        {
            message = "Amount must be greater than zero.";
            return false;
        }
        if (string.IsNullOrWhiteSpace(category))
        {
            message = "Category cannot be empty.";
            return false;
        }

        List<Expense> expenses = repository.GetAll();
        int nextId = expenses.Count == 0 ? 1 : expenses.Max(expense => expense.Id) + 1;
        repository.Add(new Expense(nextId, name, amount, category, DateTime.Now));
        message = "Expense added.";
        return true;
    }

    public List<Expense> GetAll()
    {
        return repository.GetAll().OrderBy(expense => expense.Id).ToList();
    }

    public List<Expense> GetByCategory(string category)
    {
        return repository.GetAll()
            .Where(expense => expense.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
            .OrderBy(expense => expense.Id)
            .ToList();
    }

    public bool Delete(int id)
    {
        return repository.DeleteById(id);
    }

    public decimal GetTotal()
    {
        return repository.GetAll().Sum(expense => expense.Amount);
    }
}

