# Lecture 04 - Files, JSON, Exceptions and LINQ

Lecture 04 solves the next problem: data disappears when the console application stops.

## Projects

- `Lecture04.Demo` - expense menu with automatic JSON load/save, search, filter, delete, and summary.
- `Lecture04.CodeBasics` - runnable walkthrough for paths, text files, JSON serialization/deserialization, exceptions, and LINQ.

## Main Ideas

- Memory data disappears after the process ends.
- Files provide persistent storage.
- JSON is a structured text format.
- `System.Text.Json` converts C# values to JSON and back.
- Exceptions represent runtime problems.
- `try/catch/finally` lets the program handle problems instead of crashing.
- LINQ helps query collections clearly.

## Run

```bash
dotnet run --project Code/Lecture04_Files_JSON_Exceptions_and_LINQ/Lecture04.Demo/Lecture04.Demo.csproj

dotnet run --project Code/Lecture04_Files_JSON_Exceptions_and_LINQ/Lecture04.CodeBasics/Lecture04.CodeBasics.csproj
```

After this lecture, students may save Milestone 01 - Basic Console.
