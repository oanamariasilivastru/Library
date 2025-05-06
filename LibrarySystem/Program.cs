using System;
using System.Linq;
using System.Threading.Tasks;
using LibraryConsole.Data;
using LibraryConsole.Models;
using LibraryConsole.Repositories;
using LibraryConsole.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryConsole
{
    class Program
    {
        static async Task Main()
        {
            var services = new ServiceCollection();
            services.AddDbContext<LibraryContext>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ILendingRepository, LendingRepository>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ILendingService, LendingService>();
            var provider = services.BuildServiceProvider();

            var db         = provider.GetRequiredService<LibraryContext>();
            await db.Database.EnsureCreatedAsync();

            var bookSvc    = provider.GetRequiredService<IBookService>();
            var lendingSvc = provider.GetRequiredService<ILendingService>();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Library Management ===");
                Console.WriteLine("1. Add Book");
                Console.WriteLine("2. List All Books");
                Console.WriteLine("3. Search Books");
                Console.WriteLine("4. Update Book");
                Console.WriteLine("5. Delete Book");
                Console.WriteLine("6. Lend Book");
                Console.WriteLine("7. Return Book");
                Console.WriteLine("8. List Active Loans");
                Console.WriteLine("9. List All Loans");
                Console.WriteLine("0. Exit");
                Console.Write("Choice: ");

                try
                {
                    switch (Console.ReadLine())
                    {
                        case "1": await DoAdd(bookSvc, db);        break;
                        case "2": await DoList(bookSvc);           break;
                        case "3": await DoSearch(bookSvc);         break;
                        case "4": await DoUpdate(bookSvc, db);     break;
                        case "5": await DoDelete(bookSvc, db);     break;
                        case "6": await DoLend(lendingSvc, db);    break;
                        case "7": await DoReturn(lendingSvc, db);  break;
                        case "8": await DoListActiveLoans(lendingSvc); break;
                        case "9": await DoListAllLoans(lendingSvc);   break;
                        case "0": return;
                        default:
                            Console.WriteLine("Invalid option."); Pause(); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError: {ex.Message}");
                    Pause();
                }
            }
        }

        static async Task DoAdd(IBookService svc, LibraryContext db)
        {
            Console.Write("Title: ");
            var title  = Console.ReadLine() ?? "";
            Console.Write("Author: ");
            var author = Console.ReadLine() ?? "";
            Console.Write("Quantity: ");
            int.TryParse(Console.ReadLine(), out var qty);

            var book = await svc.CreateAsync(title, author, qty);
            await db.SaveChangesAsync();
            Console.WriteLine($"\nBook added (ID: {book.Id})");
            Pause();
        }

        static async Task DoList(IBookService svc)
        {
            var books = await svc.GetAllAsync();
            Console.WriteLine();
            foreach (var b in books)
                Console.WriteLine($"{b.Id} | {b.Title} by {b.Author} (Qty: {b.Quantity})");
            Pause();
        }

        static async Task DoSearch(IBookService svc)
        {
            var criteria = new BookSearchCriteria();

            // ID filter
            Console.Write("Book ID (leave empty = all): ");
            var idInput = Console.ReadLine()?.Trim();
            if (int.TryParse(idInput, out var id))
                criteria.Id = id;

            // Title filter
            Console.Write("Title contains (leave empty = all): ");
            var titleInput = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(titleInput))
                criteria.TitleContains = titleInput;
            
            // Author filter
            Console.Write("Author contains (leave empty = all): ");
            var authorInput = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(authorInput))
                criteria.AuthorContains = authorInput;

            // Min quantity filter
            Console.Write("Min quantity (leave empty = all): ");
            var minQtyInput = Console.ReadLine()?.Trim();
            if (int.TryParse(minQtyInput, out var minQty))
                criteria.MinQuantity = minQty;

            // Max quantity filter
            Console.Write("Max quantity (leave empty = all): ");
            var maxQtyInput = Console.ReadLine()?.Trim();
            if (int.TryParse(maxQtyInput, out var maxQty))
                criteria.MaxQuantity = maxQty;

            // Delegate to service/repository for the actual filtering
            var results = await svc.SearchAsync(criteria);

            Console.WriteLine("\n--- Search Results ---");
            if (!results.Any())
            {
                Console.WriteLine("No books found.");
            }
            else
            {
                foreach (var book in results)
                    Console.WriteLine($"{book.Id} | {book.Title} by {book.Author} (Qty: {book.Quantity})");
            }

            Pause();
        }
        static async Task DoUpdate(IBookService svc, LibraryContext db)
        {
            Console.Write("Book ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
                throw new ArgumentException("Invalid numeric ID.");

            Console.Write("New Title (empty = no change): ");
            var t = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(t)) t = null;

            Console.Write("New Author (empty = no change): ");
            var a = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(a)) a = null;

            Console.Write("New Quantity (empty = no change): ");
            var qi = Console.ReadLine();
            int? qty = null;
            if (int.TryParse(qi, out var qv))
                qty = qv;

            var updated = await svc.UpdateAsync(id, t, a, qty);
            await db.SaveChangesAsync();

            Console.WriteLine("\nBook updated:");
            Console.WriteLine($"{updated.Id} | {updated.Title} by {updated.Author} (Qty: {updated.Quantity})");
            Pause();
        }

        static async Task DoDelete(IBookService svc, LibraryContext db)
        {
            Console.Write("Book ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
                throw new ArgumentException("Invalid numeric ID.");

            await svc.DeleteAsync(id);
            await db.SaveChangesAsync();
            Console.WriteLine("\nBook deleted.");
            Pause();
        }

        static async Task DoLend(ILendingService svc, LibraryContext db)
        {
            Console.Write("Book ID to lend: ");
            if (!int.TryParse(Console.ReadLine(), out var bookId))
                throw new ArgumentException("Invalid numeric ID.");

            var loan = await svc.LendAsync(bookId);
            await db.SaveChangesAsync();
            Console.WriteLine($"\nLent at {loan.DateLent:yyyy-MM-dd}, due {loan.DueDate:yyyy-MM-dd}, Loan ID: {loan.Id}");
            Pause();
        }

        static async Task DoReturn(ILendingService svc, LibraryContext db)
        {
            Console.Write("Loan ID to return: ");
            if (!int.TryParse(Console.ReadLine(), out var loanId))
                throw new ArgumentException("Invalid numeric ID.");

            await svc.ReturnAsync(loanId);
            await db.SaveChangesAsync();
            Console.WriteLine("\nBook returned successfully.");
            Pause();
        }

        static async Task DoListActiveLoans(ILendingService svc)
        {
            var loans = await svc.GetActiveAsync();
            Console.WriteLine("\n--- Active Loans ---");
            if (!loans.Any())
                Console.WriteLine("No active loans.");
            else
                foreach (var lt in loans)
                    Console.WriteLine(
                        $"{lt.Id} | {lt.Book.Title} lent {lt.DateLent:yyyy-MM-dd} due {lt.DueDate:yyyy-MM-dd}");
            Pause();
        }

        static async Task DoListAllLoans(ILendingService svc)
        {
            var loans = await svc.GetAllAsync();
            Console.WriteLine("\n--- All Loans ---");
            if (!loans.Any())
                Console.WriteLine("No loan records.");
            else
                foreach (var lt in loans)
                {
                    var returnedPart = lt.DateReturned.HasValue
                        ? $" returned {lt.DateReturned.Value:yyyy-MM-dd}"
                        : "";
                    Console.WriteLine(
                        $"{lt.Id} | {lt.Book.Title} lent {lt.DateLent:yyyy-MM-dd} due {lt.DueDate:yyyy-MM-dd}{returnedPart}");
                }
            Pause();
        }

        static void Pause()
        {
            Console.WriteLine("\nPress any key to continue…");
            Console.ReadKey(true);
        }
    }
}
