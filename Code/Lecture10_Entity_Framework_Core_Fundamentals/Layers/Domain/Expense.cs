namespace Lecture10.Domain;
public class Expense { public int Id { get; set; } public int UserId { get; set; } public string Name { get; set; } = string.Empty; public decimal Amount { get; set; } public string Category { get; set; } = string.Empty; public DateTime CreatedAt { get; set; } = DateTime.UtcNow; }
