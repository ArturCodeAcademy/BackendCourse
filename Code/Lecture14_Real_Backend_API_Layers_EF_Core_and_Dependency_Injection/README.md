# Lecture 14 - Real Backend: API, Layers, EF Core and Dependency Injection

This lesson joins the previous pieces without collapsing their boundaries.

## Dependency-injection reasoning

The Project host knows concrete classes. It registers EF `DbContext`, `EfExpenseRepository` and `ExpenseService`. Presentation receives `ExpenseService` from the framework. Application never writes `new EfExpenseRepository()` itself.

Use Scoped for `DbContext`, repository and service because one HTTP request receives one coherent unit of work.

## Request path

HTTP request -> Presentation controller action -> Application service -> repository contract -> EF repository -> SQLite

Responses travel in the reverse direction. An endpoint has no SQL. A repository has no HTTP status code. A service has no `WebApplication`.

The API preserves the contracts from Lecture 13: request DTOs are validated, response DTOs do not expose `UserId`, a `201 Created` response points to a working route, and an expense is always queried by both owner and id.

## Controller structure

`Presentation/Controllers/ExpensesController.cs` owns HTTP concerns. `[HttpGet]` and `[HttpPost]` define the verbs, `[Route(...)]` defines the resource URL, and `[ProducesResponseType]` documents `200`, `201`, `400`, and `404` responses.

`Program.cs` registers the controller assembly with `AddControllers().AddApplicationPart(...)` and enables attribute routes with `MapControllers()`. The explicit application part is needed because Presentation is a separate class library.
## Local database

At startup the lesson calls `EnsureCreatedAsync()` so a new student can run this self-contained DI demonstration without first creating a database manually. This is intentionally a disposable local database. In a production continuation, replace it with the migration workflow from Lecture 11.

The connection string comes from `Project/appsettings.json`; use an environment variable or secret storage for real production secrets.

## Guided checks

1. Follow one POST request across all five layers.
2. Create an expense and request its `Location` URL.
3. Request that id under another user id and explain the `404` ownership boundary.
4. POST invalid input and inspect the `400` validation-problem response.
5. Change the connection string in `appsettings.json`.
6. Explain why `AddDbContext` creates a scoped dependency.
7. Replace `EfExpenseRepository` with a fake implementation for a service test.
8. Replace `EnsureCreatedAsync()` with migrations from Lecture 11.
