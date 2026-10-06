using Lecture06.Demo;

Console.WriteLine("Expense Tracker - Interface Version");
Console.WriteLine("===================================");
Console.WriteLine("1. Temporary memory storage");
Console.WriteLine("2. CSV file storage");
Console.WriteLine("3. JSON file storage");
Console.WriteLine("4. Duplicated file storage");
Console.Write("Choose storage: ");
string storageChoice = Console.ReadLine();

IExpenseRepository repository;
if (storageChoice == "1")
{
    repository = new InMemoryExpenseRepository();
    Console.WriteLine("Using temporary memory storage.");
}
else if (storageChoice == "2")
{
	string dataPath = Path.Combine(Directory.GetCurrentDirectory(), "data", "expenses.csv");
	repository = new CsvExpenceRepository(dataPath);
	Console.WriteLine("Using CSV file storage: " + dataPath);
}
else if (storageChoice == "3")
{
    string dataPath = Path.Combine(Directory.GetCurrentDirectory(), "data", "expenses.json");
    repository = new JsonExpenseRepository(dataPath);
    Console.WriteLine("Using JSON file storage: " + dataPath);
}
else
{
    string jsonDataPath = Path.Combine(Directory.GetCurrentDirectory(), "data", "Duplicated.json");
    string csv1DataPath = Path.Combine(Directory.GetCurrentDirectory(), "data", "Duplicated1.csv");
    string csv2DataPath = Path.Combine(Directory.GetCurrentDirectory(), "data", "Duplicated2.csv");
    repository = new AgregatedExpenceRepository(
        new JsonExpenseRepository(jsonDataPath),
        new CsvExpenceRepository(csv1DataPath),
        new CsvExpenceRepository(csv2DataPath));
}

ExpenseTrackerApp app = new ExpenseTrackerApp(repository);
app.Run();
