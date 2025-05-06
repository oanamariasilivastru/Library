using LibraryConsole.Data;
using LibraryConsole.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryConsole.Repositories
{
    public class LendingRepository : ILendingRepository
    {
        private readonly LibraryContext _ctx;

        // Constructor: receive the EF Core DbContext via dependency injection
        public LendingRepository(LibraryContext ctx) => _ctx = ctx;

        // Retrieve a lending transaction by its integer ID
        public async Task<LendingTransaction?> GetByIdAsync(int id)
            => await _ctx.LendingTransactions.FindAsync(id);

        // Get all active loans (those not yet returned), including the related Book entity
        public async Task<IEnumerable<LendingTransaction>> GetActiveAsync()
            => await _ctx.LendingTransactions
                .Where(lt => lt.DateReturned == null)
                .Include(lt => lt.Book)
                .ToListAsync();

        // Get all lending transactions, returned or not, including the related Book
        public async Task<IEnumerable<LendingTransaction>> GetAllAsync()
            => await _ctx.LendingTransactions
                .Include(lt => lt.Book)
                .ToListAsync();

        // Add a new lending transaction to the context (to be saved later)
        public async Task AddAsync(LendingTransaction loan)
            => await _ctx.LendingTransactions.AddAsync(loan);

        // Mark an existing lending transaction as modified (e.g. when returning a book)
        public void Update(LendingTransaction loan)
            => _ctx.LendingTransactions.Update(loan);
    }
}