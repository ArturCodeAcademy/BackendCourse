using Lecture13.Domain;
using Lecture13.Presentation.Contracts;

namespace Lecture13.Presentation.Mappings;

public static class ExpenseResponseMapper
{
    public static ExpenseResponse FromEntity(Expense expense)
    {
        return new ExpenseResponse(
            expense.Id,
            expense.Name,
            expense.Amount,
            expense.Category,
            expense.CreatedAt);
    }
}