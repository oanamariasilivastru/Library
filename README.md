# Library Console App – Run & Configuration Guide

These instructions will help you configure and start the Library Console application. No additional database file is required—you can point the app at your existing SQLite database.

---

## Prerequisites

- **.NET 8 SDK** installed and on your PATH  
- (Optional) an IDE such as Visual Studio, Rider, or VS Code  

---

## Project Dependencies

Make sure your **LibraryConsole.csproj** includes the following packages:

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.0" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0">
    <PrivateAssets>all</PrivateAssets>
  </PackageReference>
  <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.0.0" />
</ItemGroup>
```
These give you:

EF Core SQLite provider

EF Core design tools (for migrations, if you choose to use them)

Dependency injection support

Build & Restore
From the solution root folder run:

bash
dotnet restore    # restore NuGet packages
dotnet build      # compile the solution

Running the Application
Start the console UI:

bash
dotnet run --project LibraryConsole

You’ll see this menu:

markdown
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
Type the number of the operation and follow the prompts.
