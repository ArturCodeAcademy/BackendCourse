using Lecture11.Domain;

namespace Lecture11.Application;

public interface IExpenseReader
{
    Task<List<Expense>> GetWithRelationsForUserAsync(int userId);
}
