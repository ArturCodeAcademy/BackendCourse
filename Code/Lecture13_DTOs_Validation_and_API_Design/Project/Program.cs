using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Lecture13.Application; using Lecture13.Infrastructure; using Lecture13.Presentation;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IExpenseRepository, InMemoryExpenseRepository>();
builder.Services.AddScoped<ExpenseService>();
var app = builder.Build(); app.MapExpenseEndpoints(); app.Run();


