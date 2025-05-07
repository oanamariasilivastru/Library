# Library Console App – Run & Configuration Guide

This file contains detailed instructions for configuring, building, and running the Library Console application, and describes the “Point 7” functionality (due‑date & return‑date tracking) that was added.

---

## Prerequisites

- **.NET 8 SDK** installed and on your PATH  
- (Optional) an IDE like Visual Studio, Rider, or VS Code  
- (Optional) **SQLite CLI** if you want to inspect the SQLite database file  

---
## Clone the Repository

To get started, clone your GitHub repo and enter its folder:

```bash
git clone https://github.com/oanamariasilivastru/Library.git
cd Library
```
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

If you have another database, replace "library.sqlite" with your desired path or filename.
Example:
```csharp
optionsBuilder.UseSqlite("Data Source=C:/data/my_library.db");
```
Note: The application will create missing tables if they do not exist. You do not need to delete or recreate your existing database file unless you want a clean slate.

### From Your IDE (e.g. Rider, Visual Studio)

1. Open the solution (`.sln`) in your IDE.  
2. In Solution Explorer, right‑click the **LibraryConsole** project and select **Set as Startup Project**.  
3. Click the **Run** (▶️) button.

Your IDE will automatically restore, build, and launch the console for you.  


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


## Usage Overview

1. **Add Book**  
   - Enter **Title**, **Author**, **Quantity** (must be ≥ 1).

2. **List All Books**  
   - Shows **ID**, **Title**, **Author**, and stock for each book.

3. **Search Books**  
   - Filter by any combination of:  
     - **Book ID** (exact match)  
     - **Title contains** (substring)  
     - **Author contains** (substring)  
     - **Min quantity**  
     - **Max quantity**

4. **Update Book**  
   - Enter **Book ID**, then new **Title**/**Author**/**Quantity** or leave blank to keep the current value.

5. **Delete Book**  
   - Enter **Book ID** to remove it.

6. **Lend Book**  
   - Enter **Book ID**. Stock is decremented and a loan record is created.

7. **Return Book**  
   - Enter **Loan ID**. Stock is restored and the return date is recorded.

8. **List Active Loans**  
   - Shows loans not yet returned, with their due dates.

9. **List All Loans**  
   - Shows full loan history, including return dates.

0. **Exit**  
   - Closes the application.

