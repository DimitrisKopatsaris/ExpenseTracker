using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Task<IReadOnlyList<Account>> GetAllAsync()
        {
            return _accountRepository.GetAllAsync();
        }

        public async Task<Account> CreateAsync(string name, decimal startingBalance)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Account name is required.", nameof(name));

            // Optional: prevent duplicate names
            var exists = await _accountRepository.ExistsByNameAsync(name);
            if (exists)
                throw new InvalidOperationException("An account with the same name already exists.");

            var account = new Account
            {
                Name = name,
                StartingBalance = startingBalance
            };

            await _accountRepository.AddAsync(account);
            await _accountRepository.SaveChangesAsync();

            return account;
        }

        public Task<Account?> GetByIdAsync(int id)
        {
            return _accountRepository.GetByIdAsync(id);
        }
        public async Task<Account?> UpdateAsync(int id, string name, decimal startingBalance)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account is null) return null;

            if ( string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Account name is required.", nameof(name));
            }

            var exists = await _accountRepository.ExistsByNameAsync(name);
            if (exists && !string.Equals(account.Name, name, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("An account with the same name already exists.");
            }

            account.Name = name;
            account.StartingBalance = startingBalance;

            _accountRepository.Update(account);
            await _accountRepository.SaveChangesAsync();

            return account;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account is null) return false;

            var hasExpenses = await _accountRepository.HasExpensesAsync(id);
            if (hasExpenses)
            {
                throw new InvalidOperationException("Account cannot be deleted, since it has related expenses.");
            }

            _accountRepository.Remove(account);
            await _accountRepository.SaveChangesAsync();

            return true;

        }
    }
}
