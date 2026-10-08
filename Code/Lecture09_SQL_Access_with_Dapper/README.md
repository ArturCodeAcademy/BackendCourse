# Lecture 09 - SQL Access with Dapper

## What changes after Lecture 08

The Application contracts remain the boundary. ExpenseService still validates input and receives IExpenseRepository; only the Infrastructure implementation changes from JSON to Dapper and SQLite.

## Solution structure

~~~text
Layers/
  Domain/          User and Expense entities
  Application/     repository contracts and ExpenseService
  Infrastructure/  SqliteDatabase, DapperUserRepository, DapperExpenseRepository
  Presentation/    console conversion and menu
Lecture09.CodeExamples/  focused parameterized-query example
Project/                 composition root and complete console app
~~~

## Why Dapper is in Infrastructure

Dapper knows SQL commands, connections, SELECT, INSERT, and parameters. Those are persistence details. ExpenseService must not know whether data comes from JSON, SQLite, PostgreSQL, or an HTTP service.

The repository query enforces ownership:

~~~sql
DELETE FROM Expenses
WHERE Id = @ExpenseId AND UserId = @UserId;
~~~

ExpenseId and UserId are parameters, not values pasted into SQL text. Parameters prevent SQL injection and preserve types.

## Run

~~~powershell
dotnet run --project .\Lecture09.CodeExamples
dotnet run --project .\Project
~~~

The project creates data/expenses.db beside its executable. The focused project seeds an alice account only to keep the lesson on Dapper rather than repeating Lecture 08 registration.

## Database-session connection

Create the same tables in the database class:

- Users(Id, Username, PasswordHash)
- Expenses(Id, UserId, Name, Amount, Category, CreatedAt)

Explain primary key, foreign key, UNIQUE username, and the CHECK (Amount > 0) constraint. Compare each database constraint with the corresponding service validation.

## Guided checks

1. Run Lecture09.CodeExamples and find the parameterized SELECT.
2. Run Project, enter alice, add two expenses, and list them.
3. Restart the project: records remain because they are in SQLite, not a list in memory.
4. Inspect the DELETE query with Id and UserId. Explain why a different user cannot delete the row.
5. Replace DapperExpenseRepository with a fake in-memory implementation without changing ExpenseService.
