using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category> CreateAsync(string name, CategoryType type);
        Task<Category?> UpdateAsync(int id, string name, CategoryType type);
        Task<bool> DeleteAsync(int id);
        Task<IReadOnlyList<Category>> GetByTypeAsync(CategoryType type);
    }
}
