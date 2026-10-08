using Lecture10.Application;
using Lecture10.Infrastructure;
using Microsoft.EntityFrameworkCore;

string folder = Path.Combine(AppContext.BaseDirectory, "data");
Directory.CreateDirectory(folder);
var options = new DbContextOptionsBuilder<ExpenseDbContext>().UseSqlite($"Data Source={Path.Combine(folder, "expenses.db")}").Options;
await using var context = new ExpenseDbContext(options);
await context.Database.EnsureCreatedAsync();

var service = new ExpenseService(new EfExpenseRepository(context));
await service.AddMineAsync(1, "EF Core coffee", 3.50m, "Food");
foreach (var expense in await service.GetMineAsync(1))
    Console.WriteLine($"{expense.Id}: {expense.Name} {expense.Amount:F2}");
