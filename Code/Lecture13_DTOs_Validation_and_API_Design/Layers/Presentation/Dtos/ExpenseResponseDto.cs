namespace Lecture13.Presentation.Dtos;

public record ExpenseResponseDto(
    int Id,
    string Name,
    decimal Amount,
    string Category,
    DateTime CreatedAt);