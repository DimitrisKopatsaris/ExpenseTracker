namespace ExpenseTracker.Application.DTOs.Accounts
{
    public class CreateAccountDto
    {
        public string Name { get; set; } = default!;
        public decimal StartingBalance { get; set; }
    }
}
