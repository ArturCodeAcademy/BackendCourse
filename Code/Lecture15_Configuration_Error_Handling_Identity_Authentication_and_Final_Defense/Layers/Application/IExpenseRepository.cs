using Lecture15.Domain;

namespace Lecture15.Application;

public interface IExpenseRepository
{
    Task<List<Expense>> GetMineAsync(string userId, CancellationToken cancellationToken);
}