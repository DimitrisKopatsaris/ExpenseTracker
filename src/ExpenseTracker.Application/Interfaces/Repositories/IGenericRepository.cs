using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Interfaces.Repositories
{
    /// <summary>
    /// Basic CRUD operations shared by all repositories.
    /// </summary>
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();

        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);

        /// <summary>
        /// Persists pending changes to the underlying data store.
        /// </summary>
        Task<int> SaveChangesAsync();
    }
}
