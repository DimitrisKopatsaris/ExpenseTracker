using System.Text.Json.Serialization;

namespace ExpenseTracker.Domain.Entities;

public class Account //Reminder for me: each account can hold many expenses, like from a wallet money can be spent on many things(expenses)
{
    public int Id { get; set; } //Every entity in EF Core must have a primary key (unique identifier).
    public string Name { get; set; } = default!; //The name of the account — e.g., "Wallet", "Bank Account", = default!; simply suppresses a compiler warning since EF Core sets this value later.
    public decimal StartingBalance { get; set; } //The amount of money the account starts with
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow; //day and time the account was created

    [JsonIgnore]
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>(); //this will tell EF Core to create a one to many relationship between Account and Expense. It makes a list with all the expenses.

}
