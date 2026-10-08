using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication; using Lecture15.Application; namespace Lecture15.Presentation;
public static class ApiEndpoints { public static void MapApiEndpoints(this WebApplication app) { app.MapGet("/api/me/expenses", async (ClaimsPrincipal user, ExpenseService service, CancellationToken ct) => { string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier); return Results.Ok(await service.GetMineAsync(userId ?? "", ct)); }).RequireAuthorization(); } }



