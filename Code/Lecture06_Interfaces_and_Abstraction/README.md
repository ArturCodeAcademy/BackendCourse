# Lecture 06 - Interfaces and Abstraction

Lecture 06 introduces a contract between the application and the way data is stored or a task is performed.

## Projects

- `Lecture06.Demo` - the expense tracker works with `IExpenseRepository`. Students can choose temporary memory storage or JSON-file storage without changing the menu code.
- `Lecture06.CodeBasics` - runnable examples of interfaces, implementations, polymorphism, and composition.

## Main Ideas

- An interface defines a contract: members that an implementing class must provide.
- A class can implement an interface using `: InterfaceName`.
- Code can depend on an interface instead of one concrete class.
- Polymorphism lets one interface variable refer to different implementations.
- Composition means an object uses another object to perform part of its work.
- An interface is useful when there is a real interchangeable behavior or boundary, not as decoration.

## Run

```bash
dotnet run --project Code/Lecture06_Interfaces_and_Abstraction/Lecture06.Demo/Lecture06.Demo.csproj

dotnet run --project Code/Lecture06_Interfaces_and_Abstraction/Lecture06.CodeBasics/Lecture06.CodeBasics.csproj
```

## Shared Database-Course Increment

Write the future SQL repository contract in plain English: list the actions the application needs from persistent storage (read all, add, delete by id). Then match each action to a future SQL statement. The C# application still uses memory or JSON in this lecture; SQL arrives later.
