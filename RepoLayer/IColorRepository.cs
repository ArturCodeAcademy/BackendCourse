using ModelLayer;

namespace RepoLayer;

public interface IColorRepository
{
	public List<Color> GetAllColors();
	public void AddColor(Color color);
	public void UpdateColor(Color color);
	public void DeleteColor(Guid colorId);
}
