using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LibraryConsole.Entities;
using LibraryConsole.Models;
using LibraryConsole.Repositories;

namespace LibraryConsole.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repo;
        public BookService(IBookRepository repo) => _repo = repo;

        public async Task<Book> CreateAsync(string title, string author, int quantity)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(title))
                errors.Add("Title must not be empty.");
            if (string.IsNullOrWhiteSpace(author))
                errors.Add("Author must not be empty.");
            // now we forbid zero or negative on creation
            if (quantity <= 0)
                errors.Add("Quantity must be at least 1.");
            if (errors.Count > 0)
                throw new ValidationException("Validation failed: " + string.Join(" ", errors));

            var book = new Book(title, author, quantity);
            await _repo.AddAsync(book);
            return book;
        }

        public Task<IEnumerable<Book>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<IEnumerable<Book>> SearchAsync(BookSearchCriteria c)
        {
            if (c.MinQuantity.HasValue && c.MaxQuantity.HasValue
                && c.MinQuantity > c.MaxQuantity)
            {
                throw new ArgumentException("MinQuantity cannot exceed MaxQuantity.");
            }

            return _repo.SearchAsync(c);
        }

        public async Task<Book> UpdateAsync(int id, string? title, string? author, int? quantity)
        {
            var book = await _repo.GetByIdAsync(id)
                       ?? throw new InvalidOperationException($"Book with ID {id} not found.");

            var errors = new List<string>();
            if (title  != null && title.Trim().Length == 0)
                errors.Add("Title must not be empty if provided.");
            if (author != null && author.Trim().Length == 0)
                errors.Add("Author must not be empty if provided.");
            // update also forbids zero or negative
            if (quantity.HasValue && quantity.Value <= 0)
                errors.Add("Quantity must be at least 1.");
            if (errors.Count > 0)
                throw new ValidationException("Validation failed: " + string.Join(" ", errors));

            if (!string.IsNullOrWhiteSpace(title))  book.Title    = title!;
            if (!string.IsNullOrWhiteSpace(author)) book.Author   = author!;
            if (quantity.HasValue)                 book.Quantity = quantity.Value;

            _repo.Update(book);
            return book;
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _repo.GetByIdAsync(id)
                       ?? throw new InvalidOperationException($"Book with ID {id} not found.");
            _repo.Delete(book);
        }
    }
}
