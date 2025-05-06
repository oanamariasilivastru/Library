using LibraryConsole.Entities;
using LibraryConsole.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryConsole.Services
{
    public interface IBookService
    {
        // Create a new book and return the created entity
        Task<Book> CreateAsync(string title, string author, int quantity);

        // Retrieve all books
        Task<IEnumerable<Book>> GetAllAsync();

        // Search for books matching the given criteria
        Task<IEnumerable<Book>> SearchAsync(BookSearchCriteria criteria);

        // Update fields of an existing book by its ID
        Task<Book> UpdateAsync(int id, string? title, string? author, int? quantity);

        // Delete a book by its ID
        Task DeleteAsync(int id);
    }
}