# Lecture 12 - HTTP and the First ASP.NET Core Web API

The console menu is replaced by HTTP endpoints. Presentation no longer calls Console.ReadLine; it maps routes, request data, status codes, and JSON responses.

## Layer roles

- Domain: Expense data only.
- Application: Add and retrieve use cases.
- Infrastructure: current in-memory repository, replaced by EF in later lectures.
- Presentation: route-to-service translation.
- Project: the ASP.NET Core host and dependency registrations.

## Endpoints

| Method | Route | Meaning | Success |
|---|---|---|---|
| GET | /api/expenses | read collection | 200 |
| GET | /api/expenses/{id} | read one resource | 200 or 404 |
| POST | /api/expenses | create resource | 201 or 400 |

Run with dotnet run --project .\Project. Use an HTTP client to call the routes.

## Guided checks

1. Request GET /api/expenses before and after POST.
2. POST an amount of zero and read the validation response.
3. Request an unknown id and explain 404.
4. Explain why endpoint code does not construct an InMemoryExpenseRepository.
5. Sketch the same HTTP calls for the future database repository.
