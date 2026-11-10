using System.ComponentModel.DataAnnotations;

namespace Lecture14.Presentation.Contracts;

public class CreateExpenseRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string? Name { get; init; }

    [Range(0.01, 1_000_000)]
    public decimal Amount { get; init; }

    [Required]
    [StringLength(50)]
    public string? Category { get; init; }
}