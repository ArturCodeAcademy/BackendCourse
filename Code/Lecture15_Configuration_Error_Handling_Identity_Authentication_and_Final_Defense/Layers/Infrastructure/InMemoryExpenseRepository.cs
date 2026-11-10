using Lecture15.Application;
using Lecture15.Domain;

namespace Lecture15.Infrastructure;

public sealed class InMemoryExpenseRepository : IExpenseRepository
{
    private readonly Dictionary<string, List<Expense>> expensesByUser = new(StringComparer.Ordinal)
    {
        ["alice"] =
        [
            new Expense
            {
                Id = 1,
                Name = "Lunch",
                Amount = 12.50m,
                Category = "Food"
            }
        ],
        ["bob"] =
        [
            new Expense
            {
                Id = 2,
                Name = "Train",
                Amount = 28.00m,
                Category = "Travel"
            }
        ]
    };

    public Task<List<Expense>> GetMineAsync(string userId, CancellationToken cancellationToken)
    {
        var expenses = expensesByUser.TryGetValue(userId, out var items)
            ? items.Select(item => new Expense
            {
                Id = item.Id,
                Name = item.Name,
                Amount = item.Amount,
                Category = item.Category
            }).ToList()
            : [];

        return Task.FromResult(expenses);
    }
}