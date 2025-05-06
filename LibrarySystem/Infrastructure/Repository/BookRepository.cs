using LibraryConsole.Data;
using LibraryConsole.Entities;
using LibraryConsole.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryConsole.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryContext _ctx;

        // inject the EF Core DbContext
        public BookRepository(LibraryContext ctx) => _ctx = ctx;

        // retrieve a single book by its numeric ID
        public async Task<Book?> GetByIdAsync(int id)
            => await _ctx.Books.FindAsync(id);

        // get all books, read-only (no EF change tracking)
        public async Task<IEnumerable<Book>> GetAllAsync()
            => await _ctx.Books
                         .AsNoTracking()
                         .ToListAsync();

        // perform a dynamic search using the given criteria
        public async Task<IEnumerable<Book>> SearchAsync(BookSearchCriteria c)
        {
            // start with the full Books query
            var q = _ctx.Books.AsQueryable();

            // filter by ID if provided
            if (c.Id.HasValue)
                q = q.Where(b => b.Id == c.Id.Value);

            // filter by title substring if provided
            if (!string.IsNullOrWhiteSpace(c.TitleContains))
                q = q.Where(b => EF.Functions.Like(
                    b.Title,
                    $"%{c.TitleContains.Trim()}%"));

            // filter by author substring if provided
            if (!string.IsNullOrWhiteSpace(c.AuthorContains))
                q = q.Where(b => EF.Functions.Like(
                    b.Author,
                    $"%{c.AuthorContains.Trim()}%"));

            // filter by minimum quantity if provided
            if (c.MinQuantity.HasValue)
                q = q.Where(b => b.Quantity >= c.MinQuantity.Value);

            // filter by maximum quantity if provided
            if (c.MaxQuantity.HasValue)
                q = q.Where(b => b.Quantity <= c.MaxQuantity.Value);

            // execute the query and return results without tracking
            return await q
                         .AsNoTracking()
                         .ToListAsync();
        }

        // add a new book to the context for insertion
        public async Task AddAsync(Book book)
            => await _ctx.Books.AddAsync(book);

        // mark an existing book as modified
        public void Update(Book book)
            => _ctx.Books.Update(book);

        // remove a book from the context
        public void Delete(Book book)
            => _ctx.Books.Remove(book);
    }
}
