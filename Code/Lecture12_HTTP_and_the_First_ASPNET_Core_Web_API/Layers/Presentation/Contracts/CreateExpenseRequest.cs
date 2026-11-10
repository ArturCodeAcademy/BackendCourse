using System.ComponentModel.DataAnnotations;

namespace Lecture12.Presentation.Contracts;

public class CreateExpenseRequest
{
    [Required]
    [StringLength(100)]
    public string? Name { get; init; }

    [Range(typeof(decimal), "0.01", "1000000")]
    public decimal Amount { get; init; }

    [Required]
    [StringLength(50)]
    public string? Category { get; init; }
}