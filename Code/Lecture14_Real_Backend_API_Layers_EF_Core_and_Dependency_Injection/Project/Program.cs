using Lecture14.Application;
using Lecture14.Infrastructure;
using Lecture14.Presentation.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["ConnectionStrings:Expenses"] ?? "Data Source=expenses.db";

builder.Services.AddControllers()
    .AddApplicationPart(typeof(ExpensesController).Assembly);

builder.Services.AddDbContext<ExpenseDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddScoped<IExpenseRepository, EfExpenseRepository>();
builder.Services.AddScoped<ExpenseService>();

var app = builder.Build();

// A self-contained local lesson needs a schema before the first HTTP request.
await using (AsyncServiceScope scope = app.Services.CreateAsyncScope())
{
    var database = scope.ServiceProvider.GetRequiredService<ExpenseDbContext>();
    await database.Database.EnsureCreatedAsync();
}

app.MapControllers();
app.Run();