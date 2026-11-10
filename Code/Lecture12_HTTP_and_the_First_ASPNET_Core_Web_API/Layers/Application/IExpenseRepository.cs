using Lecture12.Domain;

namespace Lecture12.Application;

public interface IExpenseRepository
{
    IReadOnlyList<Expense> GetAll();

    Expense? GetById(int id);

    Expense Add(Expense expense);
}