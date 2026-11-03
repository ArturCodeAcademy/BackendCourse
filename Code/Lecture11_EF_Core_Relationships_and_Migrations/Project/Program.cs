using Lecture11.Application;
using Lecture11.Domain;
using Lecture11.Infrastructure;
using Microsoft.EntityFrameworkCore;

var options = new DbContextOptionsBuilder<ExpenseDbContext>()
    .UseSqlite(DatabaseConnection.CreateSqliteConnectionString())
    .Options;

await using var context = new ExpenseDbContext(options);
await context.Database.MigrateAsync();

User? user = await context.Users.SingleOrDefaultAsync(candidate => candidate.Username == "alice");
if (user is null)
{
    user = new User { Username = "alice" };
    context.Users.Add(user);
    await context.SaveChangesAsync();
}

Category? category = await context.Categories.SingleOrDefaultAsync(candidate => candidate.Name == "Food");
if (category is null)
{
    category = new Category { Name = "Food" };
    context.Categories.Add(category);
    await context.SaveChangesAsync();
}

bool hasExpenses = await context.Expenses.AnyAsync(expense => expense.UserId == user.Id);
if (!hasExpenses)
{
    context.Expenses.Add(new Expense
    {
        UserId = user.Id,
        CategoryId = category.Id,
        Name = "Lunch",
        Amount = 12.50m
    });
    await context.SaveChangesAsync();
}

var reader = new EfExpenseReader(context);
foreach (Expense expense in await reader.GetWithRelationsForUserAsync(user.Id))
{
    Console.WriteLine($"{expense.User!.Username}: {expense.Name} / {expense.Category!.Name}");
}
