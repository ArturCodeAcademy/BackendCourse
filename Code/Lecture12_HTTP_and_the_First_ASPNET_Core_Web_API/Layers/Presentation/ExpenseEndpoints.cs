using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Lecture12.Application; namespace Lecture12.Presentation;
public record CreateExpenseRequest(string? Name, decimal Amount, string? Category);
public static class ExpenseEndpoints
{
    public static void MapExpenseEndpoints(this WebApplication app)
    {
        app.MapGet("/api/expenses", (ExpenseService service) => Results.Ok(service.GetAll()));
        app.MapGet("/api/expenses/{id:int}", (int id, ExpenseService service) => service.Get(id) is { } expense ? Results.Ok(expense) : Results.NotFound());
        app.MapPost("/api/expenses", (CreateExpenseRequest request, ExpenseService service) => { try { var expense = service.Add(request.Name, request.Amount, request.Category); return Results.Created("/api/expenses/" + expense.Id, expense); } catch (ArgumentException exception) { return Results.ValidationProblem(new Dictionary<string, string[]> { ["expense"] = [exception.Message] }); } });
    }
}


