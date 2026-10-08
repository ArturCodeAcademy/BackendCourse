namespace Lecture11.Domain;
public class User { public int Id { get; set; } public string Username { get; set; } = string.Empty; public List<Expense> Expenses { get; set; } = []; }
public class Category { public int Id { get; set; } public string Name { get; set; } = string.Empty; public List<Expense> Expenses { get; set; } = []; }
public class Expense { public int Id { get; set; } public int UserId { get; set; } public User? User { get; set; } public int CategoryId { get; set; } public Category? Category { get; set; } public string Name { get; set; } = string.Empty; public decimal Amount { get; set; } }
