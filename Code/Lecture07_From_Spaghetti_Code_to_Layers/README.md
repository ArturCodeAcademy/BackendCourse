# Lecture 07 - From Spaghetti Code to Layers

This lecture uses the same Visual Studio solution layout as the reference structure:

```text
Lecture 07 - From Spaghetti Code to Layers
|-- Layers
|   |-- Lecture07.Application
|   |-- Lecture07.Domain
|   |-- Lecture07.Infrastructure
|   `-- Lecture07.Presentation
|-- Lecture07.CodeExamples
`-- Lecture07.Project
```

## Layers

`Layers` is a solution folder containing four separate class-library projects.

- `Lecture07.Domain` contains the `Expense` model and has no reference to UI or JSON.
- `Lecture07.Application` contains `IExpenseRepository` and `ExpenseService`; it references Domain.
- `Lecture07.Infrastructure` contains `JsonExpenseRepository`; it references Application and Domain.
- `Lecture07.Presentation` contains `ConsoleMenu`; it references Application and Domain.

## Console Projects

- `Lecture07.CodeExamples` is a focused runnable demonstration of separated responsibilities.
- `Lecture07.Project` is the evolving expense tracker. It references Application, Infrastructure, and Presentation. `Program.cs` is the composition root.

## Run

```bash
dotnet run --project Code/Lecture07_From_Spaghetti_Code_to_Layers/Lecture07.CodeExamples/Lecture07.CodeExamples.csproj

dotnet run --project Code/Lecture07_From_Spaghetti_Code_to_Layers/Project/Lecture07.Project.csproj
```

## Shared Database-Course Increment

Map future database code to the same projects: Domain models the entity, Application describes required storage actions, Infrastructure contains SQL technology, and Presentation remains independent of SQL.

