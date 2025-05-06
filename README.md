# Library Console App – Run & Configuration Guide

This file contains detailed instructions for configuring, building, and running the Library Console application, and describes the “Point 7” functionality (due‑date & return‑date tracking) that was added.

---

## Prerequisites

- **.NET 8 SDK** installed and on your PATH  
- (Optional) an IDE like Visual Studio, Rider, or VS Code  
- (Optional) **SQLite CLI** if you want to inspect the SQLite database file  

---

## Project Dependencies

Ensure your `LibraryConsole.csproj` includes these package references:

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.0" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0">
    <PrivateAssets>all</PrivateAssets>
  </PackageReference>
  <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.0.0" />
</ItemGroup>
```

Configuration
By default, the application uses a local SQLite file named library.sqlite in its working directory. If you already have a database file or want to change its name/path, update the connection string:

Open LibraryConsole/Data/LibraryContext.cs

In the OnConfiguring method, locate:

```csharp
// In LibraryContext.cs
optionsBuilder.UseSqlite("Data Source=library.sqlite");
```

Replace "library.sqlite" with your desired path or filename.
Example:
```csharp
optionsBuilder.UseSqlite("Data Source=C:/data/my_library.db");
```
Note: The application will create missing tables if they do not exist. You do not need to delete or recreate your existing database file unless you want a clean slate.

Build & Restore
From the solution root, run:
```bash
dotnet restore
dotnet build
dotnet run --project LibraryConsole
```

Running the Application
To launch the console UI:
```bash
dotnet run --project LibraryConsole
```
You will see:

```bash
=== Library Management ===
1. Add Book
2. List All Books
3. Search Books
4. Update Book
5. Delete Book
6. Lend Book
7. Return Book
8. List Active Loans
9. List All Loans
0. Exit
Type the number of the operation and press Enter, then follow the prompts.
```

Point 7 Feature: Due‑Date & Return‑Date Tracking
We extended the lending process to give you full visibility into when loans are due and returned:

DateLent: timestamp when the book was checked out (DateTime.Now at lend time)

DueDate: automatically set to DateLent.AddDays(14)

DateReturned: recorded when you perform Return Book

How it appears in the console
Lend Book (option 6) displays:
```csharp
Lent at 2025-05-07, due 2025-05-21, Loan ID: 1
```

Return Book (option 7) displays:
```csharp
Book returned successfully.
```
List Active Loans (option 8) shows only unreturned loans:
```csharp
1 | The Hobbit lent 2025-05-07 due 2025-05-21
```
List All Loans (option 9) shows every loan record:
```csharp
1 | The Hobbit lent 2025-05-07 due 2025-05-21 returned 2025-05-15
2 | 1984     lent 2025-05-08 due 2025-05-22
```
This feature lets you:

Identify overdue loans by comparing DueDate to the current date

Maintain an audit trail of returns with exact return timestamps
