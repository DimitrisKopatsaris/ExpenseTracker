using System;

namespace ExpenseTracker.Application.DTOs.Accounts
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal StartingBalance { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
