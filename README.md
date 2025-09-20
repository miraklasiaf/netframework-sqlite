# Todo Console App (.NET Framework 4.7 + EF Core + SQLite)

This repository contains a simple console-based todo list application that targets **.NET Framework 4.7** and uses **Entity Framework Core** with **SQLite** for data storage.

## Project structure

- `TodoConsoleApp/` – Console application with Entity Framework Core DbContext, models, and services.
  - `TodoConsoleApp.csproj` – Project file targeting `net47` and referencing EF Core packages.
  - `Program.cs` – Entry point and user interface loop.
  - `TodoContext.cs` – EF Core `DbContext` configured for SQLite.
  - `TodoItem.cs` – Todo entity definition.
  - `TodoService.cs` – Helper service exposing CRUD operations.

## Requirements

- .NET SDK capable of building .NET Framework 4.7 projects (e.g., Visual Studio 2019 or newer on Windows).

## Getting started

1. Restore NuGet packages:
   ```bash
   dotnet restore TodoConsoleApp/TodoConsoleApp.csproj
   ```
2. Build the console app:
   ```bash
   dotnet build TodoConsoleApp/TodoConsoleApp.csproj
   ```
3. Run the application:
   ```bash
   dotnet run --project TodoConsoleApp/TodoConsoleApp.csproj
   ```

When run for the first time the application creates a `todo.db` SQLite database in the output directory. Use the menu to add tasks, mark them as completed, list existing tasks, or delete entries.

## Notes

- Entity Framework Core automatically ensures the SQLite database file exists via `Database.EnsureCreated()`.
- Tasks are timestamped using UTC and displayed in your local time zone in the console.
