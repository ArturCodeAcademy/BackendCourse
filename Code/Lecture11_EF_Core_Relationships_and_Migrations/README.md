# Lecture 11 - EF Core Relationships and Migrations

## Relationships

This lesson makes the database structure explicit:

- One User has many Expenses.
- One Category has many Expenses.
- Every Expense requires one User and one Category.

The foreign keys are Expense.UserId and Expense.CategoryId. Navigation properties help EF Core express the same connection in C#.

## Why migrations replace EnsureCreated

EnsureCreated is useful for tiny disposable demos. A real project needs a historical sequence of schema changes that can be reviewed and applied on another machine. A migration records how to move from one schema version to the next.

Commands:

~~~powershell
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project .\Layers\Infrastructure --startup-project .\Project
dotnet ef database update --project .\Layers\Infrastructure --startup-project .\Project
~~~

Do not edit a migration that has been applied to a shared database. Add a new migration instead.

## Layer ownership

- Domain owns entities and navigation properties.
- Infrastructure owns DbContext mapping, Include, foreign keys and migration configuration.
- Application owns use cases, not DbContext.
- Presentation never receives a database connection.

## Guided checks

1. Create InitialCreate and read the generated migration carefully.
2. Add a Description column to Category; create a second migration.
3. Try creating an Expense with an unknown UserId and explain the foreign-key failure.
4. Compare Include with a query that does not load Category.
5. Explain why a migration belongs in Infrastructure.
