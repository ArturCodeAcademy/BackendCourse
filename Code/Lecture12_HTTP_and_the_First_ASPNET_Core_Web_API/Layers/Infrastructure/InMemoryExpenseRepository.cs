using Lecture12.Application; using Lecture12.Domain; namespace Lecture12.Infrastructure;
public class InMemoryExpenseRepository : IExpenseRepository
{
    private readonly List<Expense> expenses = [new() { Id = 1, Name = "Coffee", Amount = 3.50m, Category = "Food" }];
    public IReadOnlyList<Expense> GetAll() => expenses; public Expense? GetById(int id) => expenses.SingleOrDefault(expense => expense.Id == id);
    public Expense Add(Expense expense) { expense.Id = expenses.Max(item => item.Id) + 1; expenses.Add(expense); return expense; }
}
