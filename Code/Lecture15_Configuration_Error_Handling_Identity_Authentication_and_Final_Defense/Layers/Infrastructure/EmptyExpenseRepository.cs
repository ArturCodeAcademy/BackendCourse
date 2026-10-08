using Lecture15.Application; using Lecture15.Domain; namespace Lecture15.Infrastructure;
public class EmptyExpenseRepository : IExpenseRepository { public Task<List<Expense>> GetMineAsync(string userId, CancellationToken cancellationToken) => Task.FromResult(new List<Expense>()); }
