# Lecture 13 - DTOs, Validation and API Design

## Why an API must not return entities directly

An entity describes internal data and persistence. An API response is a public contract. DTOs deliberately choose which fields a client can send and receive.

`ExpenseResponse` excludes `UserId`; the route identifies the user scope. `CreateExpenseRequest` contains only fields that a client may create.

## Validation layers

1. Presentation validates the HTTP request shape: required values, lengths and ranges.
2. Application validates business rules that must hold for every caller.
3. Infrastructure/database retains constraints as the final integrity boundary.

A client can bypass a browser form, so server-side validation is mandatory.

## Endpoints

| Method | Route | Success | Other result |
| --- | --- | --- | --- |
| GET | `/api/users/{userId}/expenses` | 200 | - |
| GET | `/api/users/{userId}/expenses/{expenseId}` | 200 | 404 when the expense is absent or belongs to another user |
| POST | `/api/users/{userId}/expenses` | 201 with a working `Location` header | 400 for invalid input |

## Controller structure

`Presentation/Controllers/ExpensesController.cs` is the HTTP entry point. Its attributes define the contract:

- `[Route("api/users/{userId:int}/expenses")]` defines the resource URL.
- `[HttpGet]` and `[HttpPost]` define the verb for each action.
- `[ProducesResponseType]` documents the possible HTTP responses.
- `[ApiController]` validates the DTO data annotations before the action method runs.

`Program.cs` uses `AddControllers()` and `MapControllers()`. `AddApplicationPart(...)` explicitly adds the Presentation assembly because the controller is not located in the web-host project.
## API decisions

- use nouns in routes, not route names such as `/createExpense`;
- use HTTP verbs to express the operation;
- return a `201 Created` location that a client can actually request;
- scope a single expense by both `userId` and `expenseId`, avoiding cross-user access;
- return consistent validation-problem JSON for bad input.

## Guided checks

1. POST a missing name and read the validation-problem JSON.
2. Create an expense, follow its `Location` header and receive `200`.
3. Request that expense under another `userId` and explain the `404`.
4. Confirm that a response does not expose `UserId`.
5. Add an `UpdateExpenseRequest` and decide whether PUT or PATCH fits.
