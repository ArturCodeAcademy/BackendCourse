using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Lecture12.Application;
using Lecture12.Infrastructure;
using Lecture12.Presentation;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IExpenseRepository, InMemoryExpenseRepository>();
builder.Services.AddScoped<ExpenseService>();
var app = builder.Build();
app.MapExpenseEndpoints();
app.Run();


