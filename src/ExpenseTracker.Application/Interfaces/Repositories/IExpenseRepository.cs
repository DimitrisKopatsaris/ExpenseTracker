using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces.Repositories
{
    /// <summary>
    /// Repository for reading and writing individual expenses.
    /// </summary>
    public interface IExpenseRepository : IGenericRepository<Expense>
    {
        Task<IReadOnlyList<Expense>> GetAllWithDetailsAsync();
        Task<Expense?> GetByIdWithDetailsAsync(int id);

        Task<IReadOnlyList<Expense>> GetByAccountAsync(
            int accountId,
            DateTime? from = null,
            DateTime? to   = null);

        Task<IReadOnlyList<Expense>> GetByCategoryAsync(
            int categoryId,
            DateTime? from = null,
            DateTime? to   = null);

        Task<IReadOnlyList<Expense>> GetByDateRangeAsync(
            DateTime? from,
            DateTime? to);
    }
}
