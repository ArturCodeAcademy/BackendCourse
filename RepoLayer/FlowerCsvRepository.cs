using ModelLayer;

namespace RepoLayer;

public class FlowerCsvRepository : IFlowerRepository
{
	private readonly string _filePath;
	private List<Flower> _flowers;

	public FlowerCsvRepository() : this("flowers.csv")
	{
		
	}

	public FlowerCsvRepository(string fileName)
	{
		_filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
		_flowers = new List<Flower>();
		ReadAllFlowers();
	}

	public void AddFlower(Flower flower)
	{
		_flowers.Add(flower);
		WriteAllFlowers();
	}

	public void DeleteFlower(Guid flowerId)
	{
		_flowers.RemoveAll(f => f.Id == flowerId);
		WriteAllFlowers();
	}

	public List<Flower> GetAllFlowers()
	{
		return _flowers;
	}

	public void UpdateFlower(Flower flower)
	{
		_flowers.RemoveAll(f => f.Id == flower.Id);
		_flowers.Add(flower);
		WriteAllFlowers();
	}

	private void ReadAllFlowers()
	{
		_flowers.Clear();
		if (!File.Exists(_filePath))
		{
			return;
		}

		using (var reader = new StreamReader(_filePath))
		{
			reader.ReadLine(); // Skip header line
			while (!reader.EndOfStream)
			{
				var line = reader.ReadLine();
				var values = line.Split(',');
				var flower = new Flower
				{
					Id = Guid.Parse(values[0]),
					Type = values[1],
					Amount = int.Parse(values[2]),
					ColorId = Guid.Parse(values[3])
				};
				_flowers.Add(flower);
			}
		}
	}

	private void WriteAllFlowers()
	{
		_flowers.Sort((f1, f2) => f1.Id.CompareTo(f2.Id));
		using (var writer = new StreamWriter(_filePath))
		{
			writer.WriteLine("Id,Type,Amount,ColorId");
			foreach (var flower in _flowers)
			{
				var line = $"{flower.Id},{flower.Type},{flower.Amount},{flower.ColorId}";
				writer.WriteLine(line);
			}
		}
	}
}