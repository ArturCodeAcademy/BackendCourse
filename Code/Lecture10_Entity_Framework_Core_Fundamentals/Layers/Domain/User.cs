namespace Lecture10.Domain;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property: one user can own many expenses.
    public List<Expense> Expenses { get; set; } = [];
}
