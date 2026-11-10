using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Lecture12.Application;
using Lecture12.Infrastructure;
using Lecture12.Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<IExpenseRepository, InMemoryExpenseRepository>();
builder.Services.AddScoped<ExpenseService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options => options.DocumentPath = "/openapi/v1.json");
}

app.MapGet("/", () => Results.Ok(new
{
    message = "Lecture 12 API is running.",
    endpoints = new[]
    {
        "GET /api/expenses",
        "GET /api/expenses/{id}",
        "POST /api/expenses",
        "GET /swagger"
    }
})).ExcludeFromDescription();

app.MapExpenseEndpoints();
app.Run();
