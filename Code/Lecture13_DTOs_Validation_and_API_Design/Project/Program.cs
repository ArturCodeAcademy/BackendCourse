using Lecture13.Application;
using Lecture13.Infrastructure;
using Lecture13.Presentation.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddApplicationPart(typeof(ExpensesController).Assembly);

builder.Services.AddSingleton<IExpenseRepository, InMemoryExpenseRepository>();
builder.Services.AddScoped<ExpenseService>();

var app = builder.Build();

app.MapControllers();
app.Run();