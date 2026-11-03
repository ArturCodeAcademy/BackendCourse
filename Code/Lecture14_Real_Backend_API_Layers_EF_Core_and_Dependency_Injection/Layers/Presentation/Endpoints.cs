using System.ComponentModel.DataAnnotations;
using Lecture14.Application;
using Lecture14.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Lecture14.Presentation;

public record CreateExpenseRequest(
    [property: Required, StringLength(100, MinimumLength = 2)] string? Name,
    [property: Range(0.01, 1_000_000)] decimal Amount,
    [property: Required, StringLength(50)] string? Category);

public record ExpenseResponse(int Id, string Name, decimal Amount, string Category, DateTime CreatedAt);

public static class ExpenseMappings
{
    public static ExpenseResponse ToResponse(this Expense item)
    {
        return new ExpenseResponse(item.Id, item.Name, item.Amount, item.Category, item.CreatedAt);
    }
}

public static class ExpenseEndpoints
{
    public static void MapExpenseEndpoints(this WebApplication app)
    {
        app.MapGet("/api/users/{userId:int}/expenses", async (int userId, ExpenseService service, CancellationToken cancellationToken) =>
        {
            List<Expense> expenses = await service.GetMineAsync(userId, cancellationToken);
            return Results.Ok(expenses.Select(item => item.ToResponse()));
        });

        app.MapGet("/api/users/{userId:int}/expenses/{expenseId:int}", async (int userId, int expenseId, ExpenseService service, CancellationToken cancellationToken) =>
        {
            Expense? expense = await service.GetMineByIdAsync(userId, expenseId, cancellationToken);
            return expense is null ? Results.NotFound() : Results.Ok(expense.ToResponse());
        });

        app.MapPost("/api/users/{userId:int}/expenses", async (int userId, CreateExpenseRequest request, ExpenseService service, CancellationToken cancellationToken) =>
        {
            Dictionary<string, string[]>? errors = Validate(request);
            if (errors is not null)
            {
                return Results.ValidationProblem(errors);
            }

            try
            {
                Expense item = await service.CreateAsync(userId, request.Name!, request.Amount, request.Category!, cancellationToken);
                string location = $"/api/users/{userId}/expenses/{item.Id}";
                return Results.Created(location, item.ToResponse());
            }
            catch (ArgumentException error)
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    ["expense"] = [error.Message]
                });
            }
        });
    }

    private static Dictionary<string, string[]>? Validate(CreateExpenseRequest request)
    {
        var results = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(request, new ValidationContext(request), results, true);

        return isValid
            ? null
            : results
                .GroupBy(result => result.MemberNames.FirstOrDefault() ?? "request")
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(result => result.ErrorMessage ?? "Invalid").ToArray());
    }
}
