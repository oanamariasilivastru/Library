using LibraryConsole.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryConsole.Services
{
    public interface ILendingService
    {
        // Loan a book by its numeric ID and return the created transaction
        Task<LendingTransaction> LendAsync(int bookId);

        // Mark a loan as returned by its transaction ID
        Task ReturnAsync(int lendingId);

        // Get all lending transactions that are still active (not returned)
        Task<IEnumerable<LendingTransaction>> GetActiveAsync();

        // Get all lending transactions, including those already returned
        Task<IEnumerable<LendingTransaction>> GetAllAsync();
    }
}