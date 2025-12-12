using System.Threading.Tasks;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces.Repositories
{
    /// <summary>
    /// Repository for working with accounts.
    /// </summary>
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<bool> ExistsAsync(int accountId);
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> HasExpensesAsync(int accountId);

    }
}
