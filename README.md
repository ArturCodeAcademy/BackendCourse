# Basics of Internet Technologies 2: Back-end Programming and Database Management

This repository contains the teacher course solution, lecture PDFs, PDF sources, and demo code for the course.

The course is built gradually. Only one lecture is created and verified at a time. Do not add the next lecture until the previous lecture has been reviewed and approved by the teacher.

## Selected .NET SDK

The installed SDKs were checked with:

```text
dotnet --list-sdks
```

Selected SDK for this course:

```text
.NET 10.0.302
```

Lecture demo projects target `net10.0` unless a later lecture has a specific reason to use another target framework.

## Repository Structure

```text
BackendCourse
|-- BackendCourse.sln
|-- README.md
|-- CoursePlan.md
|-- FinalProjectRequirements.md
|-- BUILD_REPORT.md
|-- Code
|   |-- Lecture01_Introduction_to_CSharp_and_DotNET`n|   |-- Lecture02_Conditions_Loops_and_Methods`n|   |-- Lecture03_Collections_and_Data_Structures`n|   `-- Lecture04_Files_JSON_Exceptions_and_LINQ
|       |-- Lecture01.Demo
|       `-- Lecture01.CodeBasics`n|   |-- Lecture02_Conditions_Loops_and_Methods`n|   |-- Lecture03_Collections_and_Data_Structures`n|   `-- Lecture04_Files_JSON_Exceptions_and_LINQ`n|       |-- Lecture02.Demo`n|       `-- Lecture02.CodeBasics`n|   |-- Lecture03_Collections_and_Data_Structures`n|   `-- Lecture04_Files_JSON_Exceptions_and_LINQ`n|       |-- Lecture03.Demo`n|       `-- Lecture03.CodeBasics`n|   `-- Lecture04_Files_JSON_Exceptions_and_LINQ`n|       |-- Lecture04.Demo`n|       `-- Lecture04.CodeBasics
|-- PDF
|   |-- Lecture01_Introduction_to_CSharp_and_DotNET`n|   |-- Lecture02_Conditions_Loops_and_Methods`n|   |-- Lecture03_Collections_and_Data_Structures`n|   `-- Lecture04_Files_JSON_Exceptions_and_LINQ
|       |-- Lecture01.Demo
|       `-- Lecture01.CodeBasics`n|   |-- Lecture02_Conditions_Loops_and_Methods`n|   |-- Lecture03_Collections_and_Data_Structures`n|   `-- Lecture04_Files_JSON_Exceptions_and_LINQ`n|       |-- Lecture02.Demo`n|       `-- Lecture02.CodeBasics`n|   |-- Lecture03_Collections_and_Data_Structures`n|   `-- Lecture04_Files_JSON_Exceptions_and_LINQ`n|       |-- Lecture03.Demo`n|       `-- Lecture03.CodeBasics`n|   `-- Lecture04_Files_JSON_Exceptions_and_LINQ`n|       |-- Lecture04.Demo`n|       `-- Lecture04.CodeBasics.pdf
|-- PDF_Sources
|   |-- Lecture01_Introduction_to_CSharp_and_DotNET`n|   |-- Lecture02_Conditions_Loops_and_Methods`n|   |-- Lecture03_Collections_and_Data_Structures`n|   `-- Lecture04_Files_JSON_Exceptions_and_LINQ
|       |-- Lecture01.Demo
|       `-- Lecture01.CodeBasics`n|   |-- Lecture02_Conditions_Loops_and_Methods`n|   |-- Lecture03_Collections_and_Data_Structures`n|   `-- Lecture04_Files_JSON_Exceptions_and_LINQ`n|       |-- Lecture02.Demo`n|       `-- Lecture02.CodeBasics`n|   |-- Lecture03_Collections_and_Data_Structures`n|   `-- Lecture04_Files_JSON_Exceptions_and_LINQ`n|       |-- Lecture03.Demo`n|       `-- Lecture03.CodeBasics`n|   `-- Lecture04_Files_JSON_Exceptions_and_LINQ`n|       |-- Lecture04.Demo`n|       `-- Lecture04.CodeBasics
`-- Resources
    |-- Diagrams
    |-- Images
    `-- Database
```

All lecture PDFs are stored only in `PDF`.
All lecture demo code is stored only in `Code`.

## Teacher Course Solution

`BackendCourse.sln` is the teacher solution used during lectures. Each lecture must be represented by a separate Visual Studio Solution Folder, for example:

```text
Lecture 01 - Introduction to C# and .NET
|-- Lecture01.Demo
`-- Lecture01.CodeBasics
```

## Student Semester Project

Each student creates a separate solution for their own semester project. The student solution is not part of `BackendCourse.sln`.

Students develop one idea throughout the semester. Assessment includes both the evolution of the project and the final backend application.




