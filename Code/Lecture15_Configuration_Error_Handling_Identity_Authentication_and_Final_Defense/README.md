# Lecture 15 - Configuration, Error Handling, Identity, Authentication and Final Defense

## Configuration

Connection strings, JWT keys, external URLs and production settings do not belong in C# source. Use appsettings.json for non-secret defaults, environment variables or secret storage for secrets, and environment-specific settings for deployment.

## Error handling

Endpoints should not each invent their own 500 response. The host uses centralized exception handling. Application throws a meaningful BusinessRuleException; Presentation/host translates it into a safe HTTP response. Do not send stack traces or secrets to clients.

## Authentication and authorization

Authentication answers: who is calling? Authorization answers: may this caller use this resource?

The runnable project uses a tiny X-Demo-User authentication handler only to make the request pipeline visible. It is not production authentication. Replace it with ASP.NET Core Identity plus a real store, secure password hashing, and token or cookie configuration in the final project.

A protected endpoint obtains the user id from claims, never from a request body or query string.

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
