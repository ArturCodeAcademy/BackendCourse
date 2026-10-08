using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using System.ComponentModel.DataAnnotations; using Lecture13.Application; using Lecture13.Domain; namespace Lecture13.Presentation;
public record ExpenseResponse(int Id, string Name, decimal Amount, string Category, DateTime CreatedAt);
public record CreateExpenseRequest([property: Required, StringLength(100, MinimumLength = 2)] string? Name, [property: Range(0.01, 1000000)] decimal Amount, [property: Required, StringLength(50)] string? Category);
public static class ExpenseMappings { public static ExpenseResponse ToResponse(this Expense item) => new(item.Id, item.Name, item.Amount, item.Category, item.CreatedAt); }
public static class Endpoints { public static void MapExpenseEndpoints(this WebApplication app) { app.MapGet("/api/users/{userId:int}/expenses", (int userId, ExpenseService service) => Results.Ok(service.GetForUser(userId).Select(item => item.ToResponse()))); app.MapPost("/api/users/{userId:int}/expenses", (int userId, CreateExpenseRequest request, ExpenseService service) => { var results = new List<ValidationResult>(); if (!Validator.TryValidateObject(request, new ValidationContext(request), results, true)) return Results.ValidationProblem(results.GroupBy(result => result.MemberNames.FirstOrDefault() ?? "request").ToDictionary(group => group.Key, group => group.Select(result => result.ErrorMessage ?? "Invalid").ToArray())); try { var item = service.Create(userId, request.Name!, request.Amount, request.Category!); return Results.Created("/api/expenses/" + item.Id, item.ToResponse()); } catch (Lecture13.Application.ValidationException error) { return Results.ValidationProblem(new Dictionary<string, string[]> { [error.Key] = [error.Message] }); } }); } }



