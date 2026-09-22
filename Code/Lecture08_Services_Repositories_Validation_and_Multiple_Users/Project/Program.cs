using Lecture08.Application;
using Lecture08.Infrastructure;
using Lecture08.Presentation;

string dataDirectory = Path.Combine(AppContext.BaseDirectory, "data");
string usersFile = Path.Combine(dataDirectory, "users.json");
string expensesFile = Path.Combine(dataDirectory, "expenses.json");

// The composition root is the only place that chooses concrete implementations.
IUserRepository users = new JsonUserRepository(usersFile);
IExpenseRepository expenses = new JsonExpenseRepository(expensesFile);
IPasswordService passwordService = new PasswordService();

var authService = new AuthService(users, passwordService);
var expenseService = new ExpenseService(expenses);

var menu = new ConsoleMenu(authService, expenseService);
menu.Run();

