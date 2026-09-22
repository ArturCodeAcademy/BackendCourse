# Lecture 08 - Services, Repositories, Validation and Multiple Users

## Solution structure

~~~
Lecture08_Services_Repositories_Validation_and_Multiple_Users/
|-- Layers/
|   |-- Domain/                  User and Expense: the business data
|   |-- Application/             interfaces, rules, validation, services
|   |-- Infrastructure/          JSON persistence and PBKDF2 password hashing
|   `-- Presentation/            console menus and input conversion
|-- Lecture08.CodeExamples/      small executable hash demonstration
`-- Project/                     executable multi-user Expense Tracker
~~~

## Why each layer exists

| Layer | Owns | Must not own |
|---|---|---|
| Domain | User, Expense, UserId ownership | console input, JSON, hashing algorithms |
| Application | use cases, validation, repository contracts | File.ReadAllText, menu rendering |
| Infrastructure | JSON files and PBKDF2 implementation | decisions about menu flow |
| Presentation | prompts, TryParse, output | password persistence and business rules |
| Project | wiring the concrete classes together | duplicate business logic |

## Run

~~~powershell
dotnet run --project .\Lecture08.CodeExamples
dotnet run --project .\Project
~~~

The Project writes users.json and expenses.json under its output data directory.

## Dedicated password service

AuthService depends on IPasswordService. PasswordService is the only class that receives a raw password and can hash or verify it. JsonUserRepository receives only the resulting PasswordHash property.

## Security rule

User.PasswordHash contains a PBKDF2 record:

~~~text
PBKDF2-SHA256$iterations$base64-salt$base64-derived-hash
~~~

The original password is not saved. Every registration receives a new random salt, so identical passwords produce different records. Login derives a candidate hash and compares it in constant time.

## Guided checks

1. Register alice with correct-horse-battery-staple, add an expense, then log out.
2. Register bob with another password. Bob's list must be empty.
3. While logged in as Bob, try to delete Alice's expense id. The operation must fail.
4. Inspect data/users.json: it must contain PasswordHash, never a Password property.
5. Try a password shorter than 8 characters and an expense amount of 0. Both are rejected by Application services.


