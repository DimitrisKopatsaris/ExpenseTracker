using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.DTOs.Categories
{
    public class UpdateCategoryDto
    {
        public string Name { get; set; } = default!;
        public CategoryType Type { get; set; } = CategoryType.Expense;
    }
}
