using System;

namespace LibraryConsole.Entities
{
    public class LendingTransaction
    {
        // Numeric primary key, auto‑incremented by EF Core / SQLite
        public int      Id            { get; set; }

        // Foreign key referencing Book.Id
        public int      BookId        { get; set; }

        // Navigation property to the borrowed book
        public Book     Book          { get; set; } = null!;

        // Date and time when the book was lent
        public DateTime DateLent      { get; set; }

        // Due date for returning the book
        public DateTime DueDate       { get; set; }

        // Actual return date (null if not yet returned)
        public DateTime? DateReturned { get; set; }
    }
}