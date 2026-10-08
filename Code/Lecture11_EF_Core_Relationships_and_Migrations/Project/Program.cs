using Lecture11.Domain;
using Lecture11.Infrastructure;
using Microsoft.EntityFrameworkCore;

string folder = Path.Combine(AppContext.BaseDirectory, "data");
Directory.CreateDirectory(folder);
var options = new DbContextOptionsBuilder<ExpenseDbContext>().UseSqlite("Data Source=" + Path.Combine(folder, "expenses.db")).Options;
await using var context = new ExpenseDbContext(options);
await context.Database.EnsureCreatedAsync();
if (!await context.Users.AnyAsync())
{
    var user = new User { Username = "alice" };
    var category = new Category { Name = "Food" };
    context.AddRange(user, category);
    await context.SaveChangesAsync();
    context.Expenses.Add(new Expense { UserId = user.Id, CategoryId = category.Id, Name = "Lunch", Amount = 12.50m });
    await context.SaveChangesAsync();
}
foreach (var expense in await context.Expenses.Include(expense => expense.User).Include(expense => expense.Category).ToListAsync())
    Console.WriteLine(expense.User!.Username + ": " + expense.Name + " / " + expense.Category!.Name);
