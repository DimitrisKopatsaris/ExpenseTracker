using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ExpenseTrackerDbContext dbContext)
            : base(dbContext)
        {
        }

        public Task<bool> ExistsAsync(int categoryId)
        {
            return _dbContext.Categories.AnyAsync(c => c.Id == categoryId);
        }

        public Task<bool> ExistsByNameAsync(string name)
        {
            return _dbContext.Categories.AnyAsync(c => c.Name == name);
        }

        public async Task<IReadOnlyList<Category>> GetByTypeAsync(CategoryType type)
        {
            return await _dbContext.Categories
                .AsNoTracking()
                .Where(c => c.Type == type)
                .ToListAsync();
        }

        public Task<bool> HasExpensesAsync(int categoryId)
        {
            return _dbContext.Expenses.AnyAsync(e => e.CategoryId == categoryId);
        }
    }
}
