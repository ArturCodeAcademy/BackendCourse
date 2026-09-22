# Lecture 05 - Classes and Objects

Lecture 05 replaces anonymous dictionary-based data with meaningful C# objects.

## Projects

- `Lecture05.Demo` - the expense tracker now stores `Expense` objects, persists them as JSON, and uses object properties and `ToString()`.
- `Lecture05.CodeBasics` - a runnable guided example of classes, fields, properties, constructors, methods, encapsulation, static members, object references, lists of objects, and constructor overloads.

## Main Ideas

- A class is a blueprint; an object is one created value from that blueprint.
- Fields store internal state; properties provide controlled access to data.
- Constructors create an object in a valid starting state.
- Instance methods describe behavior belonging to one object.
- `private` hides implementation details; `public` is the usable contract.
- `static` belongs to the class, not one object.
- `ToString()` gives an object a useful display representation.
- JSON can serialize and deserialize simple classes with public properties.

## Run

```bash
dotnet run --project Code/Lecture05_Classes_and_Objects/Lecture05.Demo/Lecture05.Demo.csproj

dotnet run --project Code/Lecture05_Classes_and_Objects/Lecture05.CodeBasics/Lecture05.CodeBasics.csproj
```

## Shared Database-Course Increment

Model the same `Expense` data as a database table: choose a primary key, define columns and types, and explain why each column belongs to the entity. The backend project remains file-based in this lecture; SQL persistence starts later.
