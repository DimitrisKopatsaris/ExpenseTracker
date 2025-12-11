using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Task<IReadOnlyList<Category>> GetAllAsync()
        {
            return _categoryRepository.GetAllAsync();
        }

        public async Task<Category> CreateAsync(string name, CategoryType type)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name is required.", nameof(name));

            var exists = await _categoryRepository.ExistsByNameAsync(name);
            if (exists)
                throw new InvalidOperationException("A category with the same name already exists.");

            var category = new Category
            {
                Name = name,
                Type = type
            };

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();

            return category;
        }

        public Task<IReadOnlyList<Category>> GetByTypeAsync(CategoryType type)
        {
            return _categoryRepository.GetByTypeAsync(type);
        }
    }
}
