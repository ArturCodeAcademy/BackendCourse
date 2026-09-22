using System.Text.Json;

class JsonDatabase
{
	private readonly string _directory = "databases";

	public string CurrentFile { get; private set; } = "";

	private readonly JsonSerializerOptions _options = new()
	{
		WriteIndented = true
	};

	public JsonDatabase()
	{
		Directory.CreateDirectory(_directory);
	}

	public List<string> GetFiles()
	{
		return Directory
			.GetFiles(_directory, "*.json")
			.Select(Path.GetFileName)
			.Where(x => x != null)
			.Cast<string>()
			.OrderBy(x => x)
			.ToList();
	}

	public void SelectFile(string fileName)
	{
		if (!fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
			fileName += ".json";

		CurrentFile = fileName;
	}

	public bool CreateFile(string fileName)
	{
		fileName = CleanFileName(fileName);

		if (string.IsNullOrWhiteSpace(fileName))
			return false;

		if (!fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
			fileName += ".json";

		string path = GetPath(fileName);

		if (File.Exists(path))
			return false;

		File.WriteAllText(path, "[]");
		CurrentFile = fileName;

		return true;
	}

	public bool DeleteFile(string fileName)
	{
		string path = GetPath(fileName);

		if (!File.Exists(path))
			return false;

		File.Delete(path);

		if (CurrentFile.Equals(fileName, StringComparison.OrdinalIgnoreCase))
			CurrentFile = "";

		return true;
	}

	public List<Expense> Load()
	{
		if (string.IsNullOrWhiteSpace(CurrentFile))
			return new List<Expense>();

		string path = GetPath(CurrentFile);

		if (!File.Exists(path))
			return new List<Expense>();

		try
		{
			string json = File.ReadAllText(path);

			if (string.IsNullOrWhiteSpace(json))
				return new List<Expense>();

			return JsonSerializer.Deserialize<List<Expense>>(json, _options)
				   ?? new List<Expense>();
		}
		catch
		{
			return new List<Expense>();
		}
	}

	public void Save(List<Expense> expenses)
	{
		if (string.IsNullOrWhiteSpace(CurrentFile))
			return;

		string path = GetPath(CurrentFile);
		string json = JsonSerializer.Serialize(expenses, _options);

		File.WriteAllText(path, json);
	}

	private string GetPath(string fileName)
	{
		return Path.Combine(_directory, fileName);
	}

	private string CleanFileName(string fileName)
	{
		foreach (char invalidChar in Path.GetInvalidFileNameChars())
		{
			fileName = fileName.Replace(invalidChar.ToString(), "");
		}

		return fileName.Trim();
	}
}
