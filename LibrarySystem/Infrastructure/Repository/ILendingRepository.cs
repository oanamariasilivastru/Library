using LibraryConsole.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryConsole.Repositories
{
    public interface ILendingRepository
    {
        // Find a lending transaction by its numeric ID
        Task<LendingTransaction?> GetByIdAsync(int id);

        // Get all lending transactions that have not yet been returned
        Task<IEnumerable<LendingTransaction>> GetActiveAsync();

        // Get all lending transactions, including those already returned
        Task<IEnumerable<LendingTransaction>> GetAllAsync();

        // Add a new lending transaction (book loan) to the context
        Task AddAsync(LendingTransaction loan);

        // Update an existing lending transaction (e.g. mark returned)
        void Update(LendingTransaction loan);
    }
}