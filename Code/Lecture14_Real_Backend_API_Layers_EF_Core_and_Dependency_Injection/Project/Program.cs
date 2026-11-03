using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Lecture14.Application;
using Lecture14.Infrastructure;
using Lecture14.Presentation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration["ConnectionStrings:Expenses"] ?? "Data Source=expenses.db";
builder.Services.AddDbContext<ExpenseDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddScoped<IExpenseRepository, EfExpenseRepository>();
builder.Services.AddScoped<ExpenseService>();

var app = builder.Build();

// A self-contained local lesson needs a schema before the first HTTP request.
await using (AsyncServiceScope scope = app.Services.CreateAsyncScope())
{
    ExpenseDbContext database = scope.ServiceProvider.GetRequiredService<ExpenseDbContext>();
    await database.Database.EnsureCreatedAsync();
}

app.MapExpenseEndpoints();
app.Run();
