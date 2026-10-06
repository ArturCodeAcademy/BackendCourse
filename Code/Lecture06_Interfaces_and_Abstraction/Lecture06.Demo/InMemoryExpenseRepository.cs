public class InMemoryExpenseRepository : IExpenseRepository
{
    private readonly List<Expense> expenses = new List<Expense>();

    public List<Expense> GetAll()
    {
        return expenses;
    }

    public void Add(Expense expense)
    {
        expenses.Add(expense);
    }

    public bool DeleteById(int id)
    {
        Expense expense = expenses.FirstOrDefault(item => item.Id == id);
        if (expense == null) return false;
        expenses.Remove(expense);
        return true;
    }
}
