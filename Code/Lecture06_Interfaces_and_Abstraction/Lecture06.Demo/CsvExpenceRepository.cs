using System.Globalization;
using System.Text.Json;

namespace Lecture06.Demo;

internal class CsvExpenceRepository : IExpenseRepository
{
	private readonly string filePath;
	private readonly List<Expense> expenses;

	public CsvExpenceRepository() : this("Default.csv")
	{
		
	}

	public CsvExpenceRepository(string path)
	{
		filePath = path;
		expenses = Load();
	}

	public List<Expense> GetAll()
	{
		return expenses;
	}

	public void Add(Expense expense)
	{
		expenses.Add(expense);
		Save();
	}

	public bool DeleteById(int id)
	{
		Expense expense = expenses.FirstOrDefault(item => item.Id == id);
		if (expense == null)
			return false;
		expenses.Remove(expense);
		Save();
		return true;
	}

	private List<Expense> Load()
	{
		List<Expense> loaded = new List<Expense>();

		try
		{
			if (!File.Exists(filePath))
				return loaded;

			string[] lines = File.ReadAllLines(filePath);

			foreach (string line in lines.Skip(1))
			{
				if (string.IsNullOrWhiteSpace(line))
					continue;

				string[] parts = line.Split(';');

				Expense expense = new Expense(
					int.Parse(parts[0]),
					parts[1],
					decimal.Parse(parts[2], CultureInfo.InvariantCulture),
					parts[3],
					DateTime.Parse(parts[4], CultureInfo.InvariantCulture)
				);

				loaded.Add(expense);
			}
		}
		catch (IOException ex)
		{
			Console.WriteLine("Could not read data: " + ex.Message);
		}
		catch
		{
			Console.WriteLine("Invalid CSV. Starting with an empty list.");
		}

		return loaded;
	}

	private void Save()
	{
		try
		{
			string directory = Path.GetDirectoryName(filePath);

			if (!string.IsNullOrEmpty(directory))
				Directory.CreateDirectory(directory);

			List<string> lines = new List<string>();

			lines.Add("Id;Name;Amount;Category;CreatedAt");

			foreach (Expense expense in expenses)
			{
				string line =
					expense.Id + ";" +
					expense.Name + ";" +
					expense.Amount.ToString(CultureInfo.InvariantCulture) + ";" +
					expense.Category + ";" +
					expense.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

				lines.Add(line);
			}

			File.WriteAllLines(filePath, lines);
		}
		catch (IOException ex)
		{
			Console.WriteLine("Could not save data: " + ex.Message);
		}
	}
}
