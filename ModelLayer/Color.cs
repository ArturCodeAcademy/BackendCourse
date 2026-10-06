namespace ModelLayer;

public class Color
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public string Name { get; set; } = string.Empty;
	public string HexCode { get; set; } = string.Empty;
}
