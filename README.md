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

optionsBuilder.UseSqlite("Data Source=library.sqlite");

Replace "library.sqlite" with your desired path or filename.
Example:

optionsBuilder.UseSqlite("Data Source=C:/data/my_library.db");

Note: The application will create missing tables if they do not exist. You do not need to delete or recreate your existing database file unless you want a clean slate.

Build & Restore
From the solution root, run:

dotnet restore    # Restores NuGet packages
dotnet build      # Compiles the solution

Running the Application
To launch the console UI:

dotnet run --project LibraryConsole

You will see:


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

