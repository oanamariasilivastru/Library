using LibraryConsole.Entities;
using LibraryConsole.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryConsole.Repositories
{
    public interface IBookRepository
    {
        // Get a book by its numeric ID
        Task<Book?> GetByIdAsync(int id);

        // Get all books
        Task<IEnumerable<Book>> GetAllAsync();

        // Search books by dynamic criteria
        Task<IEnumerable<Book>> SearchAsync(BookSearchCriteria criteria);

        // Add a new book
        Task AddAsync(Book book);

        // Update an existing book
        void Update(Book book);

        // Delete a book
        void Delete(Book book);
    }
}