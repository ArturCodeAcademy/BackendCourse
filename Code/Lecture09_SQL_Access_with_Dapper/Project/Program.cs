using Lecture09.Application;
using Lecture09.Infrastructure;
using Lecture09.Presentation;

string dataDirectory = Path.Combine(AppContext.BaseDirectory, "data");
Directory.CreateDirectory(dataDirectory);

var database = new SqliteDatabase(Path.Combine(dataDirectory, "expenses.db"));
database.Initialize();

IUserRepository users = new DapperUserRepository(database);
IExpenseRepository expenses = new DapperExpenseRepository(database);
IPasswordService passwords = new PasswordService();

var authService = new AuthService(users, passwords);
var expenseService = new ExpenseService(expenses);

new ConsoleMenu(authService, expenseService).Run();
