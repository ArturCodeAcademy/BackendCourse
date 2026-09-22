using System.Globalization;

class GUI
{
	private readonly JsonDatabase _database = new();
	private List<Expense> _expenses = new();

	public void Run()
	{
		SelectDatabase();

		while (true)
		{
			ShowMenu();
			int option = GetOption(1, 8);

			switch (option)
			{
				case 1:
					AddExpense();
					break;

				case 2:
					ListExpenses();
					break;

				case 3:
					SearchByName();
					break;

				case 4:
					FilterByCategory();
					break;

				case 5:
					DeleteExpense();
					break;

				case 6:
					ShowSummary();
					break;

				case 7:
					_database.Save(_expenses);
					SelectDatabase();
					break;

				case 8:
					_database.Save(_expenses);
					ShowExit();
					return;
			}
		}
	}

	private void SelectDatabase()
	{
		while (true)
		{
			Console.Clear();

			ShowBoxHeader("SELECT DATABASE");

			List<string> files = _database.GetFiles();

			if (files.Count > 0)
			{
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.WriteLine("Available files:");
				Console.WriteLine();

				for (int i = 0; i < files.Count; i++)
				{
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.Write($"  {i + 1}.");

					Console.ForegroundColor = ConsoleColor.White;
					Console.WriteLine($" {files[i]}");
				}
			}
			else
			{
				ShowWarning("No database files found.");
			}

			Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("  N. Create new file");

			if (files.Count > 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("  D. Delete file");
			}

			Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write("Choose file or action: ");
			Console.ResetColor();

			string input = Console.ReadLine()?.Trim() ?? "";

			if (input.Equals("N", StringComparison.OrdinalIgnoreCase))
			{
				CreateDatabase();
				return;
			}

			if (input.Equals("D", StringComparison.OrdinalIgnoreCase) && files.Count > 0)
			{
				DeleteDatabase(files);
				continue;
			}

			if (int.TryParse(input, out int selected) &&
				selected >= 1 &&
				selected <= files.Count)
			{
				_database.SelectFile(files[selected - 1]);
				_expenses = _database.Load();

				ShowSuccess($"Opened: {_database.CurrentFile}");
				Thread.Sleep(500);

				return;
			}

			ShowError("Invalid option.");
			Thread.Sleep(700);
		}
	}

	private void CreateDatabase()
	{
		while (true)
		{
			Console.Clear();
			ShowBoxHeader("CREATE DATABASE");

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write("File name: ");
			Console.ResetColor();

			string name = Console.ReadLine()?.Trim() ?? "";

			if (string.IsNullOrWhiteSpace(name))
			{
				ShowError("File name cannot be empty.");
				Thread.Sleep(700);
				continue;
			}

			if (!_database.CreateFile(name))
			{
				ShowError("A file with this name already exists.");
				Thread.Sleep(700);
				continue;
			}

			_expenses = new List<Expense>();

			Console.WriteLine();
			ShowSuccess($"Created: {_database.CurrentFile}");

			Thread.Sleep(600);
			return;
		}
	}

	private void DeleteDatabase(List<string> files)
	{
		Console.WriteLine();

		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.Write("File number to delete: ");
		Console.ResetColor();

		string input = Console.ReadLine()?.Trim() ?? "";

		if (!int.TryParse(input, out int number) ||
			number < 1 ||
			number > files.Count)
		{
			ShowError("Invalid file number.");
			Thread.Sleep(700);
			return;
		}

		string fileName = files[number - 1];

		Console.WriteLine();

		Console.ForegroundColor = ConsoleColor.Red;
		Console.Write($"Delete \"{fileName}\"? ");

		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.Write("[Y/N]: ");

		Console.ResetColor();

		ConsoleKeyInfo key = Console.ReadKey(true);

		Console.WriteLine();

		if (key.Key != ConsoleKey.Y)
		{
			ShowWarning("Deletion cancelled.");
			Thread.Sleep(600);
			return;
		}

		if (_database.DeleteFile(fileName))
			ShowSuccess("File deleted.");
		else
			ShowError("Could not delete file.");

		Thread.Sleep(700);
	}

	private void ShowMenu()
	{
		Console.Clear();

		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine("╔════════════════════════════════════╗");

		Console.Write("║");

		Console.ForegroundColor = ConsoleColor.Magenta;
		Console.Write("          EXPENSE TRACKER           ");

		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine("║");

		Console.WriteLine("╠════════════════════════════════════╣");

		PrintMenuItem("1", "Add expense");
		PrintMenuItem("2", "List expenses");
		PrintMenuItem("3", "Search by name");
		PrintMenuItem("4", "Filter by category");
		PrintMenuItem("5", "Delete expense");
		PrintMenuItem("6", "Show summary");
		PrintMenuItem("7", "Change database");
		PrintMenuItem("8", "Exit");

		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine("╚════════════════════════════════════╝");

		Console.WriteLine();

		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.Write("Database: ");

		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine(_database.CurrentFile);

		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.Write("Expenses: ");

		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine(_expenses.Count);

		Console.ResetColor();
	}

	private void PrintMenuItem(string number, string text)
	{
		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.Write("║  ");

		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.Write(number + ".");

		Console.ForegroundColor = ConsoleColor.White;
		Console.Write($" {text}");

		int padding = 29 - text.Length;
		Console.Write(new string(' ', Math.Max(0, padding)));

		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine("║");
	}

	private int GetOption(int min, int max)
	{
		while (true)
		{
			ConsoleKeyInfo info = Console.ReadKey(true);

			if (char.IsDigit(info.KeyChar))
			{
				int number = info.KeyChar - '0';

				if (number >= min && number <= max)
					return number;
			}
		}
	}

	private void AddExpense()
	{
		ShowHeader("ADD EXPENSE");

		string name = ReadRequired("Name");
		string category = ReadRequired("Category");
		decimal amount = ReadAmount();

		Expense expense = new()
		{
			Name = name,
			Category = category,
			Amount = amount,
			Date = DateTimeOffset.Now
		};

		_expenses.Add(expense);
		_database.Save(_expenses);

		Console.WriteLine();
		ShowSuccess("Expense added successfully.");

		Pause();
	}

	private void ListExpenses()
	{
		ShowHeader("ALL EXPENSES");

		if (_expenses.Count == 0)
		{
			ShowWarning("No expenses found.");
			Pause();
			return;
		}

		PrintExpenses(_expenses);
		Pause();
	}

	private void SearchByName()
	{
		ShowHeader("SEARCH BY NAME");

		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.Write("Search: ");
		Console.ResetColor();

		string search = Console.ReadLine()?.Trim() ?? "";

		List<Expense> result = _expenses
			.Where(x =>
				x.Name.Contains(
					search,
					StringComparison.OrdinalIgnoreCase))
			.ToList();

		Console.WriteLine();

		if (result.Count == 0)
		{
			ShowWarning("Nothing found.");
			Pause();
			return;
		}

		PrintExpenses(result);
		Pause();
	}

	private void FilterByCategory()
	{
		ShowHeader("FILTER BY CATEGORY");

		List<string> categories = _expenses
			.Select(x => x.Category)
			.Distinct(StringComparer.OrdinalIgnoreCase)
			.OrderBy(x => x)
			.ToList();

		if (categories.Count == 0)
		{
			ShowWarning("No categories found.");
			Pause();
			return;
		}

		for (int i = 0; i < categories.Count; i++)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write($"  {i + 1}.");

			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine($" {categories[i]}");
		}

		Console.WriteLine();

		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.Write("Category number: ");
		Console.ResetColor();

		string input = Console.ReadLine()?.Trim() ?? "";

		if (!int.TryParse(input, out int number) ||
			number < 1 ||
			number > categories.Count)
		{
			ShowError("Invalid category.");
			Pause();
			return;
		}

		string category = categories[number - 1];

		List<Expense> result = _expenses
			.Where(x =>
				x.Category.Equals(
					category,
					StringComparison.OrdinalIgnoreCase))
			.ToList();

		Console.WriteLine();

		PrintExpenses(result);
		Pause();
	}

	private void DeleteExpense()
	{
		ShowHeader("DELETE EXPENSE");

		if (_expenses.Count == 0)
		{
			ShowWarning("No expenses to delete.");
			Pause();
			return;
		}

		PrintExpenses(_expenses);

		Console.WriteLine();

		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.Write("Expense number: ");
		Console.ResetColor();

		string input = Console.ReadLine()?.Trim() ?? "";

		if (!int.TryParse(input, out int number) ||
			number < 1 ||
			number > _expenses.Count)
		{
			ShowError("Invalid expense number.");
			Pause();
			return;
		}

		Expense expense = _expenses[number - 1];

		Console.WriteLine();

		Console.ForegroundColor = ConsoleColor.Red;
		Console.Write($"Delete \"{expense.Name}\"? ");

		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.Write("[Y/N]: ");
		Console.ResetColor();

		ConsoleKeyInfo key = Console.ReadKey(true);

		Console.WriteLine();

		if (key.Key == ConsoleKey.Y)
		{
			_expenses.RemoveAt(number - 1);
			_database.Save(_expenses);

			ShowSuccess("Expense deleted.");
		}
		else
		{
			ShowWarning("Deletion cancelled.");
		}

		Pause();
	}

	private void ShowSummary()
	{
		ShowHeader("SUMMARY");

		if (_expenses.Count == 0)
		{
			ShowWarning("No expenses found.");
			Pause();
			return;
		}

		decimal total = _expenses.Sum(x => x.Amount);
		decimal average = _expenses.Average(x => x.Amount);

		Expense biggest = _expenses
			.OrderByDescending(x => x.Amount)
			.First();

		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.Write("Total: ");

		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine($"{total:N2}");

		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.Write("Expenses: ");

		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine(_expenses.Count);

		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.Write("Average: ");

		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine($"{average:N2}");

		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.Write("Largest: ");

		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.Write(biggest.Name);

		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine($"  {biggest.Amount:N2}");

		Console.WriteLine();

		Console.ForegroundColor = ConsoleColor.Magenta;
		Console.WriteLine("BY CATEGORY");

		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(new string('─', 50));

		var categories = _expenses
			.GroupBy(x => x.Category, StringComparer.OrdinalIgnoreCase)
			.Select(group => new
			{
				Category = group.Key,
				Count = group.Count(),
				Total = group.Sum(x => x.Amount)
			})
			.OrderByDescending(x => x.Total);

		foreach (var category in categories)
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write($"{Cut(category.Category, 20),-22}");

			Console.ForegroundColor = ConsoleColor.White;
			Console.Write($"{category.Count,5}");

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"{category.Total,18:N2}");
		}

		Console.ResetColor();

		Pause();
	}

	private void PrintExpenses(IEnumerable<Expense> expenses)
	{
		List<Expense> list = expenses.ToList();

		Console.ForegroundColor = ConsoleColor.DarkGray;

		Console.WriteLine(
			$"{"#",-4} {"NAME",-18} {"CATEGORY",-15} {"AMOUNT",12}   {"DATE",-16}");

		Console.WriteLine(new string('─', 72));

		for (int i = 0; i < list.Count; i++)
		{
			Expense expense = list[i];

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write($"{i + 1,-4}");

			Console.ForegroundColor = ConsoleColor.White;
			Console.Write($" {Cut(expense.Name, 17),-18}");

			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write($" {Cut(expense.Category, 14),-15}");

			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write($" {expense.Amount,12:N2}");

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine($"   {expense.Date:dd.MM.yyyy HH:mm}");
		}

		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(new string('─', 72));

		Console.ForegroundColor = ConsoleColor.White;
		Console.Write("Total: ");

		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine($"{list.Sum(x => x.Amount):N2}");

		Console.ResetColor();
	}

	private string ReadRequired(string title)
	{
		while (true)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write($"{title}: ");
			Console.ResetColor();

			string value = Console.ReadLine()?.Trim() ?? "";

			if (!string.IsNullOrWhiteSpace(value))
				return value;

			ShowError($"{title} cannot be empty.");
		}
	}

	private decimal ReadAmount()
	{
		while (true)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write("Amount: ");
			Console.ResetColor();

			string input = Console.ReadLine()?.Trim() ?? "";

			input = input.Replace(',', '.');

			if (decimal.TryParse(
					input,
					NumberStyles.Number,
					CultureInfo.InvariantCulture,
					out decimal amount)
				&& amount > 0)
			{
				return amount;
			}

			ShowError("Enter a valid positive amount.");
		}
	}

	private void ShowHeader(string title)
	{
		Console.Clear();
		ShowBoxHeader(title);

		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.Write("Database: ");

		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine(_database.CurrentFile);

		Console.ResetColor();
		Console.WriteLine();
	}

	private void ShowBoxHeader(string title)
	{
		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine("╔════════════════════════════════════════════╗");

		Console.Write("║ ");

		Console.ForegroundColor = ConsoleColor.Magenta;

		string text = Cut(title, 40);
		Console.Write(text);

		Console.Write(new string(' ', 41 - text.Length));

		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine("║");

		Console.WriteLine("╚════════════════════════════════════════════╝");

		Console.ResetColor();
	}

	private void ShowSuccess(string text)
	{
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine($"✓ {text}");
		Console.ResetColor();
	}

	private void ShowWarning(string text)
	{
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine($"! {text}");
		Console.ResetColor();
	}

	private void ShowError(string text)
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine($"✗ {text}");
		Console.ResetColor();
	}

	private void Pause()
	{
		Console.WriteLine();

		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.Write("Press any key to return...");

		Console.ResetColor();
		Console.ReadKey(true);
	}

	private void ShowExit()
	{
		Console.Clear();

		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine("╔════════════════════════════════╗");

		Console.ForegroundColor = ConsoleColor.Magenta;
		Console.WriteLine("║         SEE YOU SOON!          ║");

		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine("╚════════════════════════════════╝");

		Console.ResetColor();
	}

	private string Cut(string value, int maxLength)
	{
		if (value.Length <= maxLength)
			return value;

		return value[..(maxLength - 3)] + "...";
	}
}