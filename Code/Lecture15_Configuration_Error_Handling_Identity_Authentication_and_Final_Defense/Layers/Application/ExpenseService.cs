using Lecture15.Domain; namespace Lecture15.Application;
public class BusinessRuleException(string message) : Exception(message) { }
public interface IExpenseRepository { Task<List<Expense>> GetMineAsync(string userId, CancellationToken cancellationToken); }
public class ExpenseService(IExpenseRepository repository) { public Task<List<Expense>> GetMineAsync(string userId, CancellationToken ct) { if (string.IsNullOrWhiteSpace(userId)) throw new BusinessRuleException("Authenticated user id is required."); return repository.GetMineAsync(userId, ct); } }
