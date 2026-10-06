namespace ModelLayer;

public class Flower
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public string Type { get; set; } = string.Empty;
	public int Amount { get; set; } = 0;
	public Guid ColorId { get; set; } = Guid.Empty;
	public virtual Color? Color { get; set; }
}
