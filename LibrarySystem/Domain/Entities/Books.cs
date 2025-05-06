using System;

namespace LibraryConsole.Entities
{
    public class Book
    {
        // Primary key, auto-incremented integer
        public int Id { get; set; }

        // Title of the book
        public string Title { get; set; } = null!;

        // Name of the author
        public string Author { get; set; } = null!;

        // Number of copies available in stock
        public int Quantity { get; set; }

        // Parameterless constructor for EF Core
        public Book() { }

        // Initialize a new book with title, author, and starting quantity
        public Book(string title, string author, int quantity)
        {
            Title    = title    ?? throw new ArgumentNullException(nameof(title));
            Author   = author   ?? throw new ArgumentNullException(nameof(author));
            Quantity = quantity;
        }
    }
}