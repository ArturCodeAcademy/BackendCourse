using Lecture12.Application;
using Lecture12.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Lecture12.Presentation;

public record CreateExpenseRequest(string? Name, decimal Amount, string? Category);

public static class ExpenseEndpoints
{
    public static void MapExpenseEndpoints(this WebApplication app)
    {
        app.MapGet("/api/expenses", (ExpenseService service) => Results.Ok(service.GetAll()))
            .WithTags("Expenses")
            .WithName("GetAllExpenses")
            .WithSummary("Gets all expenses.")
            .Produces<IReadOnlyList<Expense>>(StatusCodes.Status200OK);

        app.MapGet("/api/expenses/{id:int}", (int id, ExpenseService service) =>
                service.Get(id) is { } expense ? Results.Ok(expense) : Results.NotFound())
            .WithTags("Expenses")
            .WithName("GetExpenseById")
            .WithSummary("Gets one expense by its identifier.")
            .Produces<Expense>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        app.MapPost("/api/expenses", (CreateExpenseRequest request, ExpenseService service) =>
            {
                try
                {
                    var expense = service.Add(request.Name, request.Amount, request.Category);
                    return Results.Created("/api/expenses/" + expense.Id, expense);
                }
                catch (ArgumentException exception)
                {
                    return Results.ValidationProblem(new Dictionary<string, string[]>
                    {
                        ["expense"] = [exception.Message]
                    });
                }
            })
            .WithTags("Expenses")
            .WithName("CreateExpense")
            .WithSummary("Creates one expense.")
            .Produces<Expense>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);
    }
}
