# Lecture 10 - Entity Framework Core Fundamentals

## The point of EF Core

Dapper maps SQL result rows to C# objects. EF Core adds a DbContext that tracks entities and translates LINQ queries into SQL. This reduces repeated mapping code, but it does not remove the need for a Domain model, Application rules, or repository boundary.

## Same layers, changed Infrastructure

- Domain: Expense remains a business entity.
- Application: ExpenseService still validates and still depends on IExpenseRepository.
- Infrastructure: ExpenseDbContext and EfExpenseRepository use EF Core.
- Presentation: converts console input only.
- Project: creates DbContextOptions and chooses SQLite.

## Important concepts

1. DbContext is a short-lived unit of work. Do not keep one forever in a web application.
2. DbSet<Expense> represents a queryable table-like collection.
3. LINQ is translated to SQL only while it is IQueryable. ToListAsync executes the query.
4. Add adds an entity to EF tracking. SaveChangesAsync writes the SQL command.
5. AsNoTracking is for read-only queries when change tracking is unnecessary.

## Run

~~~powershell
dotnet run --project .\Lecture10.CodeExamples
dotnet run --project .\Project
~~~

## Guided checks

1. Place a breakpoint before and after AddAsync.
2. Inspect ChangeTracker before SaveChangesAsync.
3. Add AsNoTracking to a read-only repository method and explain the result.
4. Replace EnsureCreatedAsync with migrations in Lecture 11.
