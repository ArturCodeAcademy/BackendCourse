# Final Project Requirements

The final project is the final evolution of the same project idea that the student starts during Lecture 01. A completely unrelated new project is not accepted unless the teacher approves the change separately.

## Required Features

- ASP.NET Core Web API.
- CRUD operations using GET, POST, PUT or PATCH, and DELETE.
- Relational database, preferably SQL Server.
- Entity Framework Core as the main persistence technology.
- At least one EF Core migration.
- At least three entities. `User` counts as an entity.
- At least one meaningful relationship between entities.
- Layered architecture: API -> Service -> Repository/Data Access -> Database.
- Dependency Injection for services and repositories.
- DTOs for main API contracts.
- Validation for incorrect input.
- Correct HTTP status codes: 200, 201, 204, 400, 401, 403, 404.
- Registration and login.
- Secure password handling. Plain-text passwords are forbidden.
- At least one protected endpoint.
- Ownership rules where they match the project domain.

## Final Defense

The defense has two parts.

### Part 1 - Evolution

The student shows milestone versions and explains what changed, why it changed, and what problem each new approach solved.

### Part 2 - Final Application

The student demonstrates the final backend: database, API, registration/login, CRUD, validation, user-specific data, protected operation, and architecture/code.

## Bonus Features

Bonus features may improve the grade, but they do not compensate for missing required features.

Examples: Blazor client, search, filtering, sorting, pagination, roles, tests, Dapper reporting endpoint, Swagger improvements, file upload, Docker, deployment, statistics.
