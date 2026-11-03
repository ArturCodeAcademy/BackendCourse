using System.ComponentModel.DataAnnotations;
using Lecture13.Application;
using Lecture13.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Lecture13.Presentation;

public record ExpenseResponse(int Id, string Name, decimal Amount, string Category, DateTime CreatedAt);

public record CreateExpenseRequest(
    [property: Required, StringLength(100, MinimumLength = 2)] string? Name,
    [property: Range(0.01, 1_000_000)] decimal Amount,
    [property: Required, StringLength(50)] string? Category);

public static class ExpenseMappings
{
    public static ExpenseResponse ToResponse(this Expense item)
    {
        return new ExpenseResponse(item.Id, item.Name, item.Amount, item.Category, item.CreatedAt);
    }
}

public static class Endpoints
{
    public static void MapExpenseEndpoints(this WebApplication app)
    {
        app.MapGet("/api/users/{userId:int}/expenses", (int userId, ExpenseService service) =>
        {
            return Results.Ok(service.GetForUser(userId).Select(item => item.ToResponse()));
        });

        app.MapGet("/api/users/{userId:int}/expenses/{expenseId:int}",
            (int userId, int expenseId, ExpenseService service) =>
            {
                return service.Get(userId, expenseId) is { } item
                    ? Results.Ok(item.ToResponse())
                    : Results.NotFound();
            });

        app.MapPost("/api/users/{userId:int}/expenses", (int userId, CreateExpenseRequest request, ExpenseService service) =>
        {
            Dictionary<string, string[]>? errors = Validate(request);
            if (errors is not null)
            {
                return Results.ValidationProblem(errors);
            }

            try
            {
                Expense item = service.Create(userId, request.Name!, request.Amount, request.Category!);
                string location = $"/api/users/{userId}/expenses/{item.Id}";
                return Results.Created(location, item.ToResponse());
            }
            catch (Lecture13.Application.ValidationException error)
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    [error.Key] = [error.Message]
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

