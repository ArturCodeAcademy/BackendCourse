using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Lecture14.Application; using Lecture14.Infrastructure; using Lecture14.Presentation; using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ExpenseDbContext>(options => options.UseSqlite(builder.Configuration["ConnectionStrings:Expenses"] ?? "Data Source=expenses.db"));
builder.Services.AddScoped<IExpenseRepository, EfExpenseRepository>();
builder.Services.AddScoped<ExpenseService>();
var app = builder.Build(); app.MapExpenseEndpoints(); app.Run();



