# Lecture 15 - Configuration, Error Handling, Identity, Authentication and Final Defense

## Configuration

Connection strings, JWT keys, external URLs and production settings do not belong in C# source. Use `appsettings.json` for non-secret defaults, environment variables or secret storage for secrets, and environment-specific settings for deployment.

The runnable project reads `Demo:ApplicationName` from `Project/appsettings.json` and returns it from `/health`. This is a deliberately harmless setting. Never place passwords, real tokens or production connection strings in the committed file.

## Error handling

Controller actions should not each invent their own 500 response. The host uses centralized exception handling. Application throws a meaningful `BusinessRuleException`; Presentation/host translates it into a safe HTTP response. Do not send stack traces or secrets to clients.

## Controller structure

`Presentation/Controllers` contains separate HTTP entry points. `ExpensesController` uses `[Authorize]` and reads the authenticated user id from claims. `HealthController` reads the harmless application name through `IConfiguration`. Each action uses `[HttpGet]` and `[ProducesResponseType]` to declare its HTTP contract.

`Program.cs` registers the separate Presentation assembly through `AddControllers().AddApplicationPart(...)` and enables attribute routing via `MapControllers()`.
## Authentication and authorization

Authentication answers: who is calling? Authorization answers: may this caller use this resource?

The runnable project uses a tiny `X-Demo-User` authentication handler only to make the request pipeline visible. It is not production authentication. Replace it with ASP.NET Core Identity plus a real store, secure password hashing, and token or cookie configuration in the final project.

A protected endpoint obtains the user id from claims, never from a request body or query string. Try these requests:

```powershell
# 401: no authenticated user
Invoke-RestMethod http://localhost:50105/api/me/expenses

# 200: only alice's demo expense is returned
Invoke-RestMethod http://localhost:50105/api/me/expenses -Headers @{ "X-Demo-User" = "alice" }

# 200: a different collection for bob
Invoke-RestMethod http://localhost:50105/api/me/expenses -Headers @{ "X-Demo-User" = "bob" }
```

## Final defense checklist

1. Explain the Domain, Application, Infrastructure, Presentation and Project responsibilities.
2. Show a migration and the database schema.
3. Demonstrate registration/login and ownership protection.
4. Show DTO validation and consistent errors.
5. Show configuration without exposing secrets.
6. Demonstrate one protected endpoint.
7. Explain what your next engineering improvement would be.

## Assessment connection

Evolution portfolio: 30 percent. It contains the seven milestone states and short explanations of architectural decisions.

Final project: 70 percent. It is a complete backend application with database persistence, API, validation, authentication and a defense demo.
