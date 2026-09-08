# Build Report

## Lecture 01 - Introduction to C# and .NET

Status: Completed.

Date: 2026-09-06
Selected SDK: .NET 10.0.302
Target framework: net10.0

## Files Created

- `BackendCourse.sln`
- `README.md`
- `CoursePlan.md`
- `FinalProjectRequirements.md`
- `Code/Lecture01_Introduction_to_CSharp_and_DotNET/Lecture01.Demo`
- `PDF/Lecture01_Introduction_to_CSharp_and_DotNET.pdf`
- `PDF_Sources/Lecture01_Introduction_to_CSharp_and_DotNET/generate_pdf.py`
- `Resources/Diagrams`
- `Resources/Images`
- `Resources/Database`

## Visual Studio Solution

- Solution file exists: Yes.
- Solution Folder exists: `Lecture 01 - Introduction to C# and .NET`.
- Demo project inside the Solution Folder: `Lecture01.Demo`.

## Build Checks

Commands executed:

```text
dotnet restore D:\Projects\BackendCourse\BackendCourse.sln
dotnet build D:\Projects\BackendCourse\BackendCourse.sln --no-restore
```

Result:

```text
Build succeeded.
0 Warning(s)
0 Error(s)
```

## Demo Smoke Test

Input:

```text
Coffee
4.50
EUR
```

Observed key output:

```text
You spent 4.50 EUR on Coffee.
```

## PDF Checks

- PDF generated successfully.
- PDF location is only under `PDF`.
- PDF source is under `PDF_Sources`.
- Page count: 25.
- Page size: 16:9 landscape.
- Visual inspection: Passed.
- Checked pages include title, theory diagram, semester project, topic lists, and summary.
- Code blocks are readable.
- Student Project Increment is included.
- Semester project assessment explanation is included.
- 40 possible project topics are included.

Non-fatal renderer note: Poppler reported missing display fonts for `Symbol` and `ArialUnicode`, but rendered PNG pages were produced and inspected successfully.

## Stop Rule

Lecture 01 is complete. Lecture 02 has not been created.

## PDF Fix - 2026-09-06

Fixed slide 17, `Project Evolution Milestones`:

- Adjusted long-flow diagram spacing.
- Reduced milestone box height for six-step diagrams.
- Moved the explanatory note below the final milestone block.
- Re-rendered and visually inspected slide 17 successfully.

Lecture 02 has not been created.

## Lecture 01 Code Basics Addition - 2026-09-06

Added a second runnable project for Lecture 01:

- `Lecture01.CodeBasics`
- Located under `Code/Lecture01_Introduction_to_CSharp_and_DotNET`
- Added to the Visual Studio Solution Folder `Lecture 01 - Introduction to C# and .NET`

Purpose:

- Explain variables and basic types.
- Show arithmetic operators.
- Show console input and parsing.
- Preview a simple `if/else` condition.
- Introduce small syntax sugar examples: `var`, `??`, `?:`, and string interpolation.

PDF updated:

- Added slides for the runnable code walkthrough project.
- Added code slides for variables/types, condition preview, and syntax sugar.
- Re-rendered and visually inspected the new code slides.

Verification:

```text
dotnet build D:\Projects\BackendCourse\BackendCourse.sln --no-restore
Build succeeded.
0 Warning(s)
0 Error(s)
```

Smoke test for `Lecture01.CodeBasics` completed successfully.

Lecture 02 has not been created.

## Lecture 01 Base C# Expansion - 2026-09-06

Updated Lecture 01 after teacher feedback.

Changes:

- Removed `??` from Lecture 01 code and PDF source.
- Disabled nullable warnings in the two beginner projects so Lecture 01 does not need nullable syntax yet.
- Expanded `Lecture01.CodeBasics` into a detailed runnable walkthrough suitable for a longer beginner class.
- Added detailed sections for input, conversion, parsing, TryParse, numeric types, constants, operators, precedence, formatting, simple conditions, boolean logic, and arrays.
- Added array examples: one-dimensional arrays, array initializers, indexing, Length, for, foreach, rectangular arrays, and jagged arrays.
- Updated the Lecture 01 PDF from 29 to 49 slides.
- Re-rendered and visually inspected representative high-risk slides: conversion, TryParse, conditions, array initializer, rectangular arrays, jagged arrays, and milestone diagram.

Verification:

```text
dotnet restore D:\Projects\BackendCourse\BackendCourse.sln
dotnet build D:\Projects\BackendCourse\BackendCourse.sln --no-restore
Build succeeded.
0 Warning(s)
0 Error(s)
```

Smoke tests:

- `Lecture01.Demo`: Passed.
- `Lecture01.CodeBasics`: Passed.

PDF:

- Pages: 49.
- Page size: 16:9 landscape.
- Visual inspection: Passed for representative risk pages.

Note:

- The requested reference path `D:\Projects\CAK\_CPP\_VBE` was not found on this machine, so the expansion was completed without importing that material.
- Lecture 02 has not been created.

## Lecture 02 - Conditions, Loops and Methods

Status: Completed.

Date: 2026-09-06
Selected SDK: .NET 10.0.302
Target framework: net10.0

## Files Created

- `Code/Lecture02_Conditions_Loops_and_Methods/Lecture02.Demo`
- `Code/Lecture02_Conditions_Loops_and_Methods/Lecture02.CodeBasics`
- `Code/Lecture02_Conditions_Loops_and_Methods/README.md`
- `PDF/Lecture02_Conditions_Loops_and_Methods.pdf`
- `PDF_Sources/Lecture02_Conditions_Loops_and_Methods/generate_pdf.py`

## Visual Studio Solution

- Solution Folder exists: `Lecture 02 - Conditions, Loops and Methods`.
- Demo project inside the Solution Folder: `Lecture02.Demo`.
- Code walkthrough project inside the Solution Folder: `Lecture02.CodeBasics`.

## Build Checks

Commands executed:

```text
dotnet restore D:\Projects\BackendCourse\BackendCourse.sln
dotnet build D:\Projects\BackendCourse\BackendCourse.sln --no-restore
```

Result:

```text
Build succeeded.
0 Warning(s)
0 Error(s)
```

## Demo Smoke Tests

- `Lecture02.Demo`: Passed with show/add/show/exit command flow.
- `Lecture02.CodeBasics`: Passed with positive amount validation input.

## PDF Checks

- PDF generated successfully.
- PDF location is only under `PDF`.
- PDF source is under `PDF_Sources`.
- Page count: 44.
- Page size: 16:9 landscape.
- Visual inspection: Passed for title, switch, validation, methods, and student increment slides.
- Code blocks are readable.
- Student Project Increment is included.

Non-fatal renderer note: Poppler reported missing display fonts for `Symbol` and `ArialUnicode`, but rendered PNG pages were produced and inspected successfully.

## Stop Rule

Lecture 02 is complete. Lecture 03 has not been created.

## Lecture 03 - Collections and Data Structures

Status: Completed.

Date: 2026-09-06
Selected SDK: .NET 10.0.302
Target framework: net10.0

## Files Created

- `Code/Lecture03_Collections_and_Data_Structures/Lecture03.Demo`
- `Code/Lecture03_Collections_and_Data_Structures/Lecture03.CodeBasics`
- `Code/Lecture03_Collections_and_Data_Structures/README.md`
- `PDF/Lecture03_Collections_and_Data_Structures.pdf`
- `PDF_Sources/Lecture03_Collections_and_Data_Structures/generate_pdf.py`

## Visual Studio Solution

- Solution Folder exists: `Lecture 03 - Collections and Data Structures`.
- Demo project inside the Solution Folder: `Lecture03.Demo`.
- Code walkthrough project inside the Solution Folder: `Lecture03.CodeBasics`.

## Build Checks

Commands executed:

```text
dotnet restore D:\Projects\BackendCourse\BackendCourse.sln
dotnet build D:\Projects\BackendCourse\BackendCourse.sln --no-restore
```

Result:

```text
Build succeeded.
0 Warning(s)
0 Error(s)
```

## Demo Smoke Tests

- `Lecture03.Demo`: Passed with add/list/find/summary/delete/list/exit command flow.
- `Lecture03.CodeBasics`: Passed.

## PDF Checks

- PDF generated successfully.
- PDF location is only under `PDF`.
- PDF source is under `PDF_Sources`.
- Page count: 44.
- Page size: 16:9 landscape.
- Visual inspection: Passed for title, List<T>, search, Dictionary, enum, and student increment slides.
- Code blocks are readable.
- Student Project Increment is included.

Non-fatal renderer note: Poppler reported missing display fonts for `Symbol` and `ArialUnicode`, but rendered PNG pages were produced and inspected successfully.

## Stop Rule

Lecture 03 is complete. Lecture 04 has not been created.

## Lecture 04 - Files, JSON, Exceptions and LINQ

Status: Completed.

Date: 2026-09-06
Selected SDK: .NET 10.0.302
Target framework: net10.0

## Files Created

- `Code/Lecture04_Files_JSON_Exceptions_and_LINQ/Lecture04.Demo`
- `Code/Lecture04_Files_JSON_Exceptions_and_LINQ/Lecture04.CodeBasics`
- `Code/Lecture04_Files_JSON_Exceptions_and_LINQ/README.md`
- `PDF/Lecture04_Files_JSON_Exceptions_and_LINQ.pdf`
- `PDF_Sources/Lecture04_Files_JSON_Exceptions_and_LINQ/generate_pdf.py`

## Visual Studio Solution

- Solution Folder exists: `Lecture 04 - Files, JSON, Exceptions and LINQ`.
- Demo project inside the Solution Folder: `Lecture04.Demo`.
- Code walkthrough project inside the Solution Folder: `Lecture04.CodeBasics`.

## Build Checks

Commands executed:

```text
dotnet restore D:\Projects\BackendCourse\BackendCourse.sln
dotnet build D:\Projects\BackendCourse\BackendCourse.sln --no-restore
```

Result:

```text
Build succeeded.
0 Warning(s)
0 Error(s)
```

## Demo Smoke Tests

- `Lecture04.CodeBasics`: Passed. Created text and JSON files, loaded them, handled expected exceptions, and ran LINQ examples.
- `Lecture04.Demo`: Passed with add/add/list/search/filter/summary/exit command flow.
- JSON persistence verified by reading the generated `data/expenses.json` file.
- Temporary smoke-test `data` folders were removed after verification.

## PDF Checks

- PDF generated successfully.
- PDF location is only under `PDF`.
- PDF source is under `PDF_Sources`.
- Page count: 43.
- Page size: 16:9 landscape.
- Visual inspection: Passed for title, deserialization, invalid JSON, LINQ Where, live demo menu, and Milestone 01 slides.
- Code blocks are readable.
- Student Project Increment is included.
- Milestone 01 - Basic Console is included.

Non-fatal renderer note: Poppler reported missing display fonts for `Symbol` and `ArialUnicode`, but rendered PNG pages were produced and inspected successfully.

## Stop Rule

Lecture 04 is complete. Lecture 05 has not been created.
