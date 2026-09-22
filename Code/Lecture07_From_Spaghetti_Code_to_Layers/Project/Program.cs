using Lecture07.Application;
using Lecture07.Infrastructure;
using Lecture07.Presentation;

string dataPath = Path.Combine(Directory.GetCurrentDirectory(), "data", "expenses.json");
IExpenseRepository repository = new JsonExpenseRepository(dataPath);
ExpenseService service = new ExpenseService(repository);
ConsoleMenu menu = new ConsoleMenu(service);

Console.WriteLine("Layered Expense Tracker");
Console.WriteLine("=======================");
Console.WriteLine("Data file: " + dataPath);
menu.Run();

