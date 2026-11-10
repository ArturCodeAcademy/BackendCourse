using Lecture15.Domain;

namespace Lecture15.Application;

public class ExpenseService
{
    private readonly IExpenseRepository repository;

    public ExpenseService(IExpenseRepository repository)
    {
        this.repository = repository;
    }

    public Task<List<Expense>> GetMineAsync(string userId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new BusinessRuleException("Authenticated user id is required.");
        }

        return repository.GetMineAsync(userId, cancellationToken);
    }
}