# Lecture 14 - Real Backend: API, Layers, EF Core and Dependency Injection

This lesson joins the previous pieces without collapsing their boundaries.

## Dependency-injection reasoning

The Project host knows concrete classes. It registers EF DbContext, EfExpenseRepository and ExpenseService. Presentation receives ExpenseService from the framework. Application never writes new EfExpenseRepository itself.

Use Scoped for DbContext, repository and service because one HTTP request receives one coherent unit of work.

## Request path

HTTP request -> Presentation endpoint -> Application service -> repository contract -> EF repository -> SQLite

Responses travel in the reverse direction. An endpoint has no SQL. A repository has no HTTP status code. A service has no WebApplication.

## Guided checks

1. Follow one POST request across all five layers.
2. Change the connection string in appsettings.json.
3. Explain why AddDbContext creates a scoped dependency.
4. Replace EfExpenseRepository with a fake implementation for a service test.
5. Add a cancellation token to a new endpoint.
