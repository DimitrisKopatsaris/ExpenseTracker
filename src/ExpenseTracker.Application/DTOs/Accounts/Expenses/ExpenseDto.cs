using System;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.DTOs.Expenses
{
    public class ExpenseDto
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }
        public DateTime OccurredOnUtc { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAtUtc { get; set; }

        public int AccountId { get; set; }
        public string AccountName { get; set; } = default!;

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = default!;
        public CategoryType CategoryType { get; set; }
    }
}
