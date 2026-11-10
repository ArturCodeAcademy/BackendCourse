namespace Lecture14.Presentation.Contracts;

public record ExpenseResponse(
    int Id,
    string Name,
    decimal Amount,
    string Category,
    DateTime CreatedAt);