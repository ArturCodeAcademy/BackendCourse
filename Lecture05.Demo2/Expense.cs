class Expense
{
	public string Name { get; set; } = "";
	public string Category { get; set; } = "";
	public decimal Amount { get; set; }
	public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
}