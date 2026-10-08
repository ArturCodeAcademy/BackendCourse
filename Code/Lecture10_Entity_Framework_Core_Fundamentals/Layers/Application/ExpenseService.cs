using Lecture10.Domain;
namespace Lecture10.Application;
public interface IExpenseRepository { Task<List<Expense>> GetForUserAsync(int userId); Task AddAsync(Expense expense); }
public class ExpenseService
{
    private readonly IExpenseRepository repository;
    public ExpenseService(IExpenseRepository repository) => this.repository = repository;
    public Task<List<Expense>> GetMineAsync(int userId) => repository.GetForUserAsync(userId);
    public Task AddMineAsync(int userId, string name, decimal amount, string category)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.");
        if (amount <= 0) throw new ArgumentException("Amount must be positive.");
        return repository.AddAsync(new Expense { UserId = userId, Name = name.Trim(), Amount = amount, Category = category.Trim() });
    }
}
