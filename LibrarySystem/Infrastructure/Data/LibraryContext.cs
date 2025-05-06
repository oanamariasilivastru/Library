using LibraryConsole.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryConsole.Data
{
    public class LibraryContext : DbContext
    {
        // Represents the Books table
        public DbSet<Book> Books { get; set; } = null!;

        // Represents the LendingTransactions table
        public DbSet<LendingTransaction> LendingTransactions { get; set; } = null!;

        // Configure the database provider and connection string
        protected override void OnConfiguring(DbContextOptionsBuilder opts)
        {
            // Use SQLite with a local file named library.sqlite
            opts.UseSqlite("Data Source=library.sqlite");
        }

        // Configure entity mappings and relationships
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuration for the Book entity
            builder.Entity<Book>(b =>
            {
                // Primary key
                b.HasKey(x => x.Id);
                // Auto-increment integer key
                b.Property(x => x.Id).ValueGeneratedOnAdd();
                // Title is required
                b.Property(x => x.Title).IsRequired();
                // Author is required
                b.Property(x => x.Author).IsRequired();
                // Quantity is required
                b.Property(x => x.Quantity).IsRequired();
            });

            // Configuration for the LendingTransaction entity
            builder.Entity<LendingTransaction>(lt =>
            {
                // Primary key
                lt.HasKey(x => x.Id);
                // Auto-increment integer key
                lt.Property(x => x.Id).ValueGeneratedOnAdd();

                // Each lending transaction is related to one book
                lt.HasOne(x => x.Book)
                  .WithMany()
                  .HasForeignKey(x => x.BookId)
                  .OnDelete(DeleteBehavior.Cascade); // remove transactions if book is deleted

                // Date the book was lent is required
                lt.Property(x => x.DateLent).IsRequired();
                // Due date for return is required
                lt.Property(x => x.DueDate).IsRequired();
                // DateReturned can be null until the book is returned
                lt.Property(x => x.DateReturned);
            });
        }
    }
}
