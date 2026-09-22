namespace Lecture08.Domain;

public class Expense
{
    public int Id { get; set; }

    // Ownership: an expense belongs to exactly one registered user.
    public int UserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string Category { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public override string ToString() =>
        $"{Id}. {Name} | {Amount:F2} | {Category} | {CreatedAt:yyyy-MM-dd}";
}
