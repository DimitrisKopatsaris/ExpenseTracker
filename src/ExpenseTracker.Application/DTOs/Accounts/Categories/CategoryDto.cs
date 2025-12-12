using System;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.DTOs.Categories
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public CategoryType Type { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
