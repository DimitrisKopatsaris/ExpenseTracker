using System;

namespace ExpenseTracker.Application.DTOs.Expenses
{
    public class CreateExpenseDto
    {
        public int AccountId { get; set; }
        public int CategoryId { get; set; }

        public decimal Amount { get; set; }
        public string? Note { get; set; }
        public DateTime OccurredOnUtc { get; set; }
    }
}
