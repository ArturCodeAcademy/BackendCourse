using Lecture07.Domain;

namespace Lecture07.Application;

public interface IExpenseRepository
{
    List<Expense> GetAll();
    void Add(Expense expense);
    bool DeleteById(int id);
}

