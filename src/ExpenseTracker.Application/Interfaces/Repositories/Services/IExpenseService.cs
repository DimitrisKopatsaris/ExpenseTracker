using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces.Services
{
    public interface IExpenseService
    {
        Task<IReadOnlyList<Expense>> GetAllAsync();
        Task<Expense?> GetByIdAsync(int id);

        Task<IReadOnlyList<Expense>> GetByAccountAsync(int accountId, DateTime? from, DateTime? to);
        Task<IReadOnlyList<Expense>> GetByCategoryAsync(int categoryId, DateTime? from, DateTime? to);
        Task<IReadOnlyList<Expense>> GetByDateRangeAsync(DateTime? from, DateTime? to);

        Task<Expense> CreateAsync(
            int accountId,
            int categoryId,
            decimal amount,
            string? note,
            DateTime occurredOnUtc);

        Task<Expense?> UpdateAsync(
            int id,
            int accountId,
            int categoryId,
            decimal amount,
            string? note,
            DateTime occuredOnUtc);
        Task<bool> DeleteAsync(int id);
    }
}
