using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.DTOs.Categories
{
    public class CreateCategoryDto
    {
        public string Name { get; set; } = default!;
        public CategoryType Type { get; set; } = CategoryType.Expense;
    }
}
