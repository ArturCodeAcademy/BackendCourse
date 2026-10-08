using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Lecture14.Application; namespace Lecture14.Presentation;
public record CreateExpenseRequest(string? Name, decimal Amount, string? Category);
public static class ExpenseEndpoints { public static void MapExpenseEndpoints(this WebApplication app) { app.MapGet("/api/users/{userId:int}/expenses", (int userId, ExpenseService service, CancellationToken ct) => service.GetMineAsync(userId, ct)); app.MapPost("/api/users/{userId:int}/expenses", async (int userId, CreateExpenseRequest request, ExpenseService service, CancellationToken ct) => { var item = await service.CreateAsync(userId, request.Name ?? "", request.Amount, request.Category ?? "", ct); return Results.Created("/api/expenses/" + item.Id, item); }); } }


