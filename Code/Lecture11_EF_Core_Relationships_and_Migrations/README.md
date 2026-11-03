# Lecture 11 - EF Core Relationships and Migrations

## Relationships

This lesson makes the database structure explicit:

- One User has many Expenses.
- One Category has many Expenses.
- Every Expense requires one User and one Category.

The foreign keys are `Expense.UserId` and `Expense.CategoryId`. Navigation properties express the same connection in C#: `Expense.User`, `Expense.Category`, `User.Expenses` and `Category.Expenses`.

`UserId` and `CategoryId` become real columns in the database. Navigation properties do **not** become duplicate columns; they let EF Core understand and load the relationship.

## What changed from Lecture 10

Lecture 10 used `EnsureCreatedAsync` for a disposable EF Core demonstration. This lecture switches to `Database.MigrateAsync()` and contains an initial migration. The project and EF tooling use the same SQLite database path through `DatabaseConnection`.

The root project creates the `DbContext` and composes objects. The query itself goes through the Application contract `IExpenseReader`, implemented by `EfExpenseReader` in Infrastructure. Its `Include` calls explicitly load `User` and `Category` for each expense.

## Migrations

A migration is source-controlled history of schema changes. It records how to move a database from one version to the next, so another developer and a deployment environment can reproduce the same schema.

This lecture includes a local tool manifest that pins `dotnet-ef` to `10.0.1`, matching the EF Core packages. Before using the commands on a new machine, restore the tool:

```powershell
dotnet tool restore
```

The first migration is already in `Layers/Infrastructure/Migrations`. Apply it with:

```powershell
dotnet tool run dotnet-ef database update --project .\Layers\Infrastructure --startup-project .\Project
```

After a model change, create and apply a new migration:

```powershell
dotnet tool run dotnet-ef migrations add AddCategoryDescription --project .\Layers\Infrastructure --startup-project .\Project
dotnet tool run dotnet-ef database update --project .\Layers\Infrastructure --startup-project .\Project
```

Do not edit a migration that has already been applied to a shared database. Add a new migration instead.

## Layer ownership

- **Domain** owns entities and navigation properties.
- **Application** owns use cases and contracts, not `DbContext`.
- **Infrastructure** owns `DbContext`, relationship mapping, `Include`, migrations and repository implementations.
- **Project** is the composition root and chooses the database provider.
- **Presentation** never receives a database connection.

## Run

```powershell
dotnet tool restore
dotnet run --project .\Lecture11.CodeExamples
dotnet run --project .\Project
```

## Guided checks

1. Read the supplied `InitialCreate` migration. Find the `Users`, `Categories`, `Expenses` and `__EFMigrationsHistory` tables it creates.
2. Add a `Description` property to `Category`, then create an `AddCategoryDescription` migration.
3. Try creating an `Expense` with an unknown `UserId` and explain the foreign-key failure.
4. Temporarily remove `Include(expense => expense.Category)` from `EfExpenseReader`; explain why `expense.Category` is no longer available in the result.
5. Explain why a migration belongs in Infrastructure and why the Application project does not reference EF Core.
