using System.Threading.Tasks;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(ExpenseTrackerDbContext dbContext)
            : base(dbContext)
        {
        }

        public Task<bool> ExistsAsync(int accountId)
        {
            return _dbContext.Accounts.AnyAsync(a => a.Id == accountId);
        }

        public Task<bool> ExistsByNameAsync(string name)
        {
            return _dbContext.Accounts.AnyAsync(a => a.Name == name);
        }

        public Task<bool> HasExpensesAsync(int accountId)
        {
            return _dbContext.Expenses.AnyAsync(e => e.AccountId == accountId);
        }
    }
}
