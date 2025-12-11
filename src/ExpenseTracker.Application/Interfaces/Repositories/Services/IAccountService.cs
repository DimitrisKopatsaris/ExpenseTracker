using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<IReadOnlyList<Account>> GetAllAsync();
        Task<Account> CreateAsync(string name, decimal startingBalance);
    }
}
