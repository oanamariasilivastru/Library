using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryConsole.Entities;
using LibraryConsole.Repositories;

namespace LibraryConsole.Services
{
    public class LendingService : ILendingService
    {
        private readonly IBookRepository _bookRepo;
        private readonly ILendingRepository _lendRepo;
        private const int LoanPeriodDays = 14;

        // receive repositories via dependency injection
        public LendingService(IBookRepository bookRepo, ILendingRepository lendRepo)
        {
            _bookRepo = bookRepo;
            _lendRepo = lendRepo;
        }

        // loan a book: ensure it exists and is in stock, decrement stock, create LendingTransaction
        public async Task<LendingTransaction> LendAsync(int bookId)
        {
            var book = await _bookRepo.GetByIdAsync(bookId)
                       ?? throw new InvalidOperationException("Book not found.");

            if (book.Quantity <= 0)
                throw new InvalidOperationException("Cannot lend: no stock available.");

            // reduce available quantity
            book.Quantity--;
            _bookRepo.Update(book);

            var now = DateTime.Now;
            var loan = new LendingTransaction
            {
                BookId       = bookId,
                DateLent     = now,
                DueDate      = now.AddDays(LoanPeriodDays),
                DateReturned = null
            };

            // add the loan record
            await _lendRepo.AddAsync(loan);
            return loan;
        }

        // return a book: ensure the loan exists and isn’t already returned, restore stock, mark returned
        public async Task ReturnAsync(int lendingId)
        {
            var loan = await _lendRepo.GetByIdAsync(lendingId)
                       ?? throw new InvalidOperationException("Lending record not found.");

            if (loan.DateReturned != null)
                throw new InvalidOperationException("This book has already been returned.");

            var book = await _bookRepo.GetByIdAsync(loan.BookId)
                       ?? throw new InvalidOperationException("Book not found.");

            // restore the book quantity
            book.Quantity++;
            _bookRepo.Update(book);

            // set the return date
            loan.DateReturned = DateTime.Now;
            _lendRepo.Update(loan);
        }

        // fetch all loans that have not yet been returned
        public Task<IEnumerable<LendingTransaction>> GetActiveAsync()
            => _lendRepo.GetActiveAsync();

        // fetch all loans, including those already returned
        public Task<IEnumerable<LendingTransaction>> GetAllAsync()
            => _lendRepo.GetAllAsync();
    }
}
