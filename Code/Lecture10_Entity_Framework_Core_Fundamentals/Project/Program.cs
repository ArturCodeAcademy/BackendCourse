using Lecture10.Application;
using Lecture10.Domain;
using Lecture10.Infrastructure;
using Microsoft.EntityFrameworkCore;

const string demoUsername = "ef-demo-user";

string folder = Path.Combine(AppContext.BaseDirectory, "data");
Directory.CreateDirectory(folder);
string databasePath = Path.Combine(folder, "expenses.db");
string connectionString = $"Data Source={databasePath}";

var options = new DbContextOptionsBuilder<ExpenseDbContext>()
    .UseSqlite(connectionString)
    .Options;

await using var context = new ExpenseDbContext(options);
await context.Database.EnsureCreatedAsync();

var userRepository = new EfUserRepository(context);
var expenseRepository = new EfExpenseRepository(context);
var userService = new UserService(userRepository);
var expenseService = new ExpenseService(expenseRepository);

User? user = await userService.FindByUsernameAsync(demoUsername);
if (user is null)
{
    user = await userService.RegisterAsync(demoUsername);
    Console.WriteLine($"Created user: {user.Username} (Id: {user.Id})");
}
else
{
    Console.WriteLine($"Found user: {user.Username} (Id: {user.Id})");
}

List<Lecture10.Domain.Expense> expenses = await expenseService.GetMineAsync(user.Id);
if (expenses.Count == 0)
{
    await expenseService.AddMineAsync(user.Id, "EF Core coffee", 3.50m, "Food");
    Console.WriteLine("Added one expense for the demo user.");
    expenses = await expenseService.GetMineAsync(user.Id);
}

Console.WriteLine($"\nExpenses owned by {user.Username}:");
foreach (var expense in expenses)
{
    Console.WriteLine($"{expense.Id}: {expense.Name} - {expense.Amount:F2} ({expense.Category})");
}
