# Lecture 12 - HTTP and the First ASP.NET Core Web API

The console menu is replaced by HTTP endpoints. Presentation no longer calls Console.ReadLine; it maps routes, request data, status codes, and JSON responses.

## Layer roles

- Domain: Expense data only.
- Application: Add and retrieve use cases.
- Infrastructure: current in-memory repository, replaced by EF in later lectures.
- Presentation: controllers translate HTTP requests into service calls and HTTP responses.
- Project: the ASP.NET Core host and dependency registrations.

## Endpoints

| Method | Route | Meaning | Success |
|---|---|---|---|
| GET | / | API status and available routes | 200 |
| GET | /api/expenses | read collection | 200 |
| GET | /api/expenses/{id} | read one resource | 200 or 404 |
| POST | /api/expenses | create resource | 201 or 400 |

Run with `dotnet run --project .\Project`. In the Development environment, the browser opens Swagger UI at `/swagger`.

## Controllers and attributes

`Presentation/Controllers/ExpensesController.cs` is the HTTP entry point. It uses the conventional controller style:

- `[ApiController]` enables API behavior, including automatic 400 responses when the request model violates data annotations.
- `[Route("api/expenses")]` defines the base URL.
- `[HttpGet]` and `[HttpPost]` define the HTTP verb for each action.
- `[ProducesResponseType]` documents possible responses for Swagger and for API consumers.

`Program.cs` registers controllers with `AddControllers()` and exposes their attribute routes with `MapControllers()`. `AddApplicationPart(...)` is explicit because the controllers live in the separate Presentation project rather than in the web-host project.
## Swagger UI and OpenAPI

- `/swagger` is an interactive page for exploring and calling the API. Open an operation, select **Try it out**, enter data, and select **Execute**.
- `/openapi/v1.json` is the generated machine-readable OpenAPI description used by Swagger UI and client-generation tools.
- Swagger UI is deliberately enabled only in the Development environment. An application should not automatically expose its internal API map in production.

Swagger describes the HTTP contract; it does not implement business rules. `ExpenseService` still validates the request and the endpoint still returns the appropriate HTTP status code.

## Guided checks

1. Request GET /api/expenses before and after POST.
2. POST an amount of zero and read the validation response.
3. Request an unknown id and explain 404.
4. Explain why endpoint code does not construct an InMemoryExpenseRepository.
5. Sketch the same HTTP calls for the future database repository.
