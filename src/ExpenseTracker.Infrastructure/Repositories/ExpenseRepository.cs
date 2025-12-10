using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Repositories
{
    public class ExpenseRepository : GenericRepository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(ExpenseTrackerDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<Expense>> GetAllWithDetailsAsync()
        {
            return await _dbContext.Expenses
                .AsNoTracking()
                .Include(e => e.Account)
                .Include(e => e.Category)
                .ToListAsync();
        }

        public async Task<Expense?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbContext.Expenses
                .AsNoTracking()
                .Include(e => e.Account)
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IReadOnlyList<Expense>> GetByAccountAsync(
            int accountId,
            DateTime? from = null,
            DateTime? to   = null)
        {
            var query = _dbContext.Expenses
                .AsNoTracking()
                .Where(e => e.AccountId == accountId);

            query = ApplyDateFilter(query, from, to);

            return await query
                .Include(e => e.Category)
                .OrderByDescending(e => e.OccurredOnUtc)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Expense>> GetByCategoryAsync(
            int categoryId,
            DateTime? from = null,
            DateTime? to   = null)
        {
            var query = _dbContext.Expenses
                .AsNoTracking()
                .Where(e => e.CategoryId == categoryId);

            query = ApplyDateFilter(query, from, to);

            return await query
                .Include(e => e.Account)
                .OrderByDescending(e => e.OccurredOnUtc)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Expense>> GetByDateRangeAsync(
            DateTime? from,
            DateTime? to)
        {
            var query = _dbContext.Expenses
                .AsNoTracking()
                .AsQueryable();

            query = ApplyDateFilter(query, from, to);

            return await query
                .Include(e => e.Account)
                .Include(e => e.Category)
                .OrderByDescending(e => e.OccurredOnUtc)
                .ToListAsync();
        }

        private static IQueryable<Expense> ApplyDateFilter(
            IQueryable<Expense> query,
            DateTime? from,
            DateTime? to)
        {
            if (from.HasValue)
                query = query.Where(e => e.OccurredOnUtc >= from.Value);

            if (to.HasValue)
                query = query.Where(e => e.OccurredOnUtc <= to.Value);

            return query;
        }
    }
}
