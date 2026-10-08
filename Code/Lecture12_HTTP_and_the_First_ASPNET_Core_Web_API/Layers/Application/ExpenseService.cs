using Lecture12.Domain; namespace Lecture12.Application;
public interface IExpenseRepository { IReadOnlyList<Expense> GetAll(); Expense? GetById(int id); Expense Add(Expense expense); }
public class ExpenseService
{
    private readonly IExpenseRepository repository; public ExpenseService(IExpenseRepository repository) => this.repository = repository;
    public IReadOnlyList<Expense> GetAll() => repository.GetAll(); public Expense? Get(int id) => repository.GetById(id);
    public Expense Add(string? name, decimal amount, string? category)
    { if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(category) || amount <= 0) throw new ArgumentException("Name, positive amount, and category are required."); return repository.Add(new Expense { Name = name.Trim(), Amount = amount, Category = category.Trim() }); }
}
