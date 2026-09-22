using System.Globalization;

namespace Lecture07.Domain;

public class Expense
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public string Category { get; set; }
    public DateTime CreatedAt { get; set; }

    public Expense()
    {
    }

    public Expense(int id, string name, decimal amount, string category, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Amount = amount;
        Category = category;
        CreatedAt = createdAt;
    }

    public override string ToString()
    {
        return Id + ". " + Name + " - " + Amount.ToString("F2", CultureInfo.InvariantCulture) + " EUR - " + Category;
    }
}


