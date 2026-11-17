using Lecture13.Domain;
using Lecture13.Presentation.Dtos;

namespace Lecture13.Presentation.Mappings;

public static class ExpenseDtoMapper
{
    public static ExpenseResponseDto FromEntity(Expense expense)
    {
        return new ExpenseResponseDto(
            expense.Id,
            expense.Name,
            expense.Amount,
            expense.Category,
            expense.CreatedAt);
    }
}