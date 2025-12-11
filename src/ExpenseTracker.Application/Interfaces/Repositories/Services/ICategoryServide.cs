using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<Category>> GetAllAsync();
        Task<Category> CreateAsync(string name, CategoryType type);
        Task<IReadOnlyList<Category>> GetByTypeAsync(CategoryType type);
    }
}
