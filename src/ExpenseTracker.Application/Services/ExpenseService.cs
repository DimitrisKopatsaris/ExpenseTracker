using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ExpenseService(IExpenseRepository expenseRepository,IAccountRepository accountRepository,ICategoryRepository categoryRepository)
        {
            _expenseRepository = expenseRepository;
            _accountRepository = accountRepository;
            _categoryRepository = categoryRepository;
        }

        public Task<IReadOnlyList<Expense>> GetAllAsync()
        {
            return _expenseRepository.GetAllAsync();
        }

        public Task<Expense?> GetByIdAsync(int id)
        {
            return _expenseRepository.GetByIdAsync(id);
        }

        public Task<IReadOnlyList<Expense>> GetByAccountAsync(int accountId, DateTime? from, DateTime? to)
        {
            return _expenseRepository.GetByAccountAsync(accountId, from, to);
        }

        public Task<IReadOnlyList<Expense>> GetByCategoryAsync(int categoryId, DateTime? from, DateTime? to)
        {
            return _expenseRepository.GetByCategoryAsync(categoryId, from, to);
        }

          public Task<IReadOnlyList<Expense>> GetByDateRangeAsync(DateTime? from, DateTime? to)
        {
            return _expenseRepository.GetByDateRangeAsync(from, to);
        }


        public async Task<Expense> CreateAsync(
            int accountId,
            int categoryId,
            decimal amount,
            string? note,
            DateTime occurredOnUtc)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than 0.");

            var accountExists = await _accountRepository.ExistsAsync(accountId);
            var categoryExists = await _categoryRepository.ExistsAsync(categoryId);

            if (!accountExists || !categoryExists)
                throw new InvalidOperationException("Invalid AccountId or CategoryId.");

            var expense = new Expense
            {
                AccountId = accountId,
                CategoryId = categoryId,
                Amount = amount,
                Note = note,
                OccurredOnUtc = occurredOnUtc
            };

            await _expenseRepository.AddAsync(expense);
            await _expenseRepository.SaveChangesAsync();

            var full = await _expenseRepository.GetByIdWithDetailsAsync(expense.Id);
            return full ?? expense;
        }

        public async Task<Expense?> UpdateAsync(
            int id,
            int accountId,
            int categoryId,
            decimal amount,
            string? note,
            DateTime occuredOnUtc
        ) 
        {
          if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException("Amount must be greater  that 0.", nameof(amount));
            }  
          var expense = await _expenseRepository.GetByIdAsync(id);
          if (expense is null) return null;

          var accountExists = await _accountRepository.ExistsAsync(accountId);
          var categoryExists = await _categoryRepository.ExistsAsync(categoryId);

          if (!accountExists || !categoryExists)
            {
                throw new InvalidOperationException("Invalid AccountId or CategoryId");
            } 

          expense.AccountId = accountId;
          expense.CategoryId = categoryId;
          expense.Amount = amount;
          expense.Note = note;
          expense.OccurredOnUtc = occuredOnUtc;

          _expenseRepository.Update(expense);
          await _expenseRepository.SaveChangesAsync();

          var full = await _expenseRepository.GetByIdWithDetailsAsync(expense.Id);
          return full ?? expense;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);
            if(expense is null) return false;

            _expenseRepository.Remove(expense);
            await _expenseRepository.SaveChangesAsync();

            return true;
        }
    }
}
