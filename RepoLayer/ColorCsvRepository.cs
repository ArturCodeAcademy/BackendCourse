using ModelLayer;

namespace RepoLayer;

public class ColorCsvRepository : IColorRepository
{
	private readonly string _filePath;
	private readonly List<Color> _colors;

	public ColorCsvRepository() : this("colors.csv")
	{
		
	}

	public ColorCsvRepository(string fileName)
	{
		_filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
		_colors = new List<Color>();
		ReadAllColors();
	}

	public void AddColor(Color color)
	{
		_colors.Add(color);
		WriteAllColors();
	}

	public void DeleteColor(Guid colorId)
	{
		_colors.RemoveAll(c => c.Id == colorId);
		WriteAllColors();
	}

	public List<Color> GetAllColors()
	{
		return _colors;
	}

	public void UpdateColor(Color color)
	{
		_colors.RemoveAll(c => c.Id == color.Id);
		_colors.Add(color);
		WriteAllColors();
	}

	private void ReadAllColors()
	{
		_colors.Clear();
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
				var color = new Color
				{
					Id = Guid.Parse(values[0]),
					Name = values[1],
					HexCode = values[2]
				};
				_colors.Add(color);
			}
		}
	}

	private void WriteAllColors()
	{
		_colors.Sort((f1, f2) => f1.Id.CompareTo(f2.Id));
		using (var writer = new StreamWriter(_filePath))
		{
			writer.WriteLine("Id,Name,HexCode");
			foreach (var color in _colors)
			{
				var line = $"{color.Id},{color.Name},{color.HexCode}";
				writer.WriteLine(line);
			}
		}
	}
}