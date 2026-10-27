# Lecture 10 - Entity Framework Core Fundamentals

## Why this lecture has both User and Expense

An expense cannot belong to an unexplained `UserId = 1`. A multi-user application first persists a real `User` record, receives its database-generated `Id`, and then stores that number in `Expense.UserId`.

This lecture therefore has two entities:

- `User`: `Id`, `Username`, `CreatedAt` and a collection of expenses.
- `Expense`: `Id`, `UserId`, `Name`, `Amount`, `Category`, `CreatedAt`.

`User` is intentionally not an authentication implementation here. Lecture 09 already introduced password hashing; never add a plain-text password merely to make a database demo shorter.

## Learning goals

By the end of the lesson, a student should be able to:

1. Explain the difference between raw SQL, Dapper and EF Core.
2. Create a connection string and open a SQLite connection.
3. Execute parameterized `INSERT`, `SELECT`, `UPDATE` and `DELETE` commands.
4. Explain `DbContext`, `DbSet<TEntity>`, LINQ, change tracking and `SaveChangesAsync`.
5. Persist a `User`, then persist and query that user's expenses.
6. Explain why a repository belongs to Infrastructure and business validation belongs to Application.

## Two parts of the lecture

### `Lecture10.CodeExamples`: raw database work

This is a small, self-contained console program. It has no menu, layers, user interaction, EF Core or application domain. It uses `Microsoft.Data.Sqlite` directly so students can see the actual building blocks:

1. connection string;
2. `SqliteConnection`;
3. `SqliteCommand`;
4. command parameters;
5. `ExecuteNonQueryAsync` for `INSERT`, `UPDATE`, `DELETE`;
6. `ExecuteReaderAsync` for `SELECT`.

It creates a `Books` table so the example is separate from the course project.

### `Project`: EF Core over the course domain

The full project keeps the same layers used in previous lectures:

- **Domain** owns `User` and `Expense` entities.
- **Application** owns use cases and validation through `UserService` and `ExpenseService`.
- **Infrastructure** owns `ExpenseDbContext`, EF Core mappings and EF repository implementations.
- **Presentation** remains free of database dependencies.
- **Project** is the composition root: it chooses SQLite, creates the `DbContext`, wires objects together and runs a small deterministic demonstration.

The project creates or finds `ef-demo-user`. It then loads only that user's expenses. There is no hidden `UserId = 1`.

## EF Core concepts to emphasize

- `DbContext` is a short-lived unit of work: create it for one operation or request, then dispose it.
- `DbSet<User>` and `DbSet<Expense>` are EF Core entry points for querying and changing tables.
- LINQ becomes SQL only when EF executes it, for example at `ToListAsync`, `SingleOrDefaultAsync` or `SaveChangesAsync`.
- `Add` changes the state of an entity to `Added`; `SaveChangesAsync` sends SQL to the database and gives a new entity its generated `Id`.
- `AsNoTracking` is useful for a read-only query because EF does not need to remember the result for later updates.
- `UserId` is the foreign key. `User.Expenses` and `Expense.User` are navigation properties. Lecture 11 expands this topic with categories, `Include`, migrations and schema evolution.
- `EnsureCreatedAsync` is convenient for this disposable classroom demo. Real team projects use migrations, which begin in Lecture 11.

## Packages

For the raw Code Examples project:

```powershell
dotnet add .\Lecture10.CodeExamples package Microsoft.Data.Sqlite
```

For the EF Core layer:

```powershell
dotnet add .\Layers\Infrastructure package Microsoft.EntityFrameworkCore.Sqlite
```

## Run

```powershell
dotnet run --project .\Lecture10.CodeExamples
dotnet run --project .\Project
```

The first command creates `library-demo.db` next to its output directory. The second creates `expenses.db` in the project's output `data` directory.

## Teaching plan: 3 hours

| Time | What to teach and do |
| --- | --- |
| 00:00-00:15 | Bridge from Lecture 09: Dapper executes the SQL we write; EF Core maps C# objects and LINQ to SQL. Explain that neither tool removes the need to understand a database. |
| 00:15-00:35 | Run `Lecture10.CodeExamples`. Read the connection string, open the connection, inspect one parameterized `INSERT`, then run `SELECT`, `UPDATE` and `DELETE`. Stress that values go through parameters, never string concatenation. |
| 00:35-01:05 | Introduce the EF Core vocabulary: package, provider, `DbContext`, `DbSet`, entity and connection string. Compare the `Books` SQL program with the EF project. |
| 01:05-01:15 | Break. |
| 01:15-01:45 | Walk through `User` first: database-generated `Id`, unique username, `UserId` on `Expense`, navigation properties and why `UserId = 1` without a real user is a bad demo. |
| 01:45-02:10 | Live-code or trace `Add`, `SaveChangesAsync`, `SingleOrDefaultAsync`, `Where` and `ToListAsync`. Put a breakpoint before and after `SaveChangesAsync` and show how `Id` appears. |
| 02:10-02:20 | Break. |
| 02:20-02:40 | Walk through the project layers. Show that `Application` knows repository interfaces, while only `Infrastructure` knows `DbContext` and EF Core. Run the project twice: first run creates the user and expense; second run finds the persisted user. |
| 02:40-02:55 | Discuss tracking versus `AsNoTracking`, `EnsureCreatedAsync` versus migrations, and why the next lesson is needed for `Include`, richer relationships and schema changes. |
| 02:55-03:00 | Give the practice task and ask each student to explain the journey from `user.Id` to `expense.UserId` in one sentence. |

## Short practice task

1. Add a `DisplayName` property to `User` and configure its maximum length in `ExpenseDbContext`.
2. Add a second expense for `ef-demo-user`.
3. Add a repository method that returns one expense by `Id` and `UserId` together.
4. Explain why querying only by `Id` can become a multi-user security problem.

Do not create a migration yet. Record the limitation of `EnsureCreatedAsync`; Lecture 11 replaces it with migrations.
