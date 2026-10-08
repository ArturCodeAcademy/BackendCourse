using Lecture09.Application;
using Lecture09.Domain;
using Lecture09.Infrastructure;
using Lecture09.Presentation;

string dataDirectory = Path.Combine(AppContext.BaseDirectory, "data");
Directory.CreateDirectory(dataDirectory);

var database = new SqliteDatabase(Path.Combine(dataDirectory, "expenses.db"));
database.Initialize();

var users = new DapperUserRepository(database);
if (users.FindByUsername("alice") is null)
{
    var passwords = new PasswordService();
    users.Add(new User { Username = "alice", PasswordHash = passwords.Hash("not-used-by-this-demo") });
}

IExpenseRepository expenses = new DapperExpenseRepository(database);
new ConsoleMenu(users, new ExpenseService(expenses)).Run();
