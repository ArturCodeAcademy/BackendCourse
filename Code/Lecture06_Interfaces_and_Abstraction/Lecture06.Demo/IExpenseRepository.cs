public interface IExpenseRepository
{
    List<Expense> GetAll();
    void Add(Expense expense);
    bool DeleteById(int id);
}
