using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces.Repositories
{
    /// <summary>
    /// Repository for expense / income categories.
    /// </summary>
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<bool> ExistsAsync(int categoryId);
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> HasExpensesAsync(int categoryId);
        Task<IReadOnlyList<Category>> GetByTypeAsync(CategoryType type);
    }
}
