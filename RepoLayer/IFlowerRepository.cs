using ModelLayer;

namespace RepoLayer;

public interface IFlowerRepository
{
	public List<Flower> GetAllFlowers();
	public void AddFlower(Flower flower);
	public void UpdateFlower(Flower flower);
	public void DeleteFlower(Guid flowerId);
}
