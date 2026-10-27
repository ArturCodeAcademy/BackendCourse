using Lecture10.Domain;

namespace Lecture10.Application;

public interface IExpenseRepository
{
    Task<List<Expense>> GetForUserAsync(int userId);
    Task AddAsync(Expense expense);
}
