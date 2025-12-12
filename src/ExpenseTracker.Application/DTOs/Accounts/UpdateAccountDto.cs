namespace ExpenseTracker.Application.DTOs.Accounts
{
    public class UpdateAccountDto
    {
        public string Name { get; set; } = default!;
        public decimal StartingBalance { get; set; }
    }
}
