using Lecture14.Domain;
using Lecture14.Presentation.Contracts;

namespace Lecture14.Presentation.Mappings;

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