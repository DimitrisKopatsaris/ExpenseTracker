namespace ExpenseTracker.Domain.Entities; //same domain layer with Account and Category.

public class Expense //This entity represents a single transaction or spending record, like bought coffee for 5euro, it is the bridge that connects one Account to one spending Category. 
{
    public int Id { get; set; } //primary key of the expense

    public int AccountId { get; set; } //this is the foreign key for the related Account, so that it knows from which account it will take money. It is the numeric key stored in SQL.
    public Account Account { get; set; } = default!; //this connects to the actual account object, together with account Id they are a foreign key pair.

    public int CategoryId { get; set; }
    public Category Category { get; set; } = default!; //same idea with account, establish the connection with a category and its ID.

    public decimal Amount { get; set; } //how much money this expense costed.
    public DateTime OccurredOnUtc { get; set; } //when it happened but not when it was added as a record
    public string? Note { get; set; } //it gives an option to make a description of the expense, like coffe with mom
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow; //it is separate from OccuredOnUtc because it may be differ when it happened and when it was recorded.
}
