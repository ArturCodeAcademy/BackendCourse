namespace Lecture13.Presentation.Contracts;

public record ExpenseResponse(
    int Id,
    string Name,
    decimal Amount,
    string Category,
    DateTime CreatedAt);