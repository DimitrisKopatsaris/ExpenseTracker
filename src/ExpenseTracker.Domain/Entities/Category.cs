using System.Text.Json.Serialization; //we use this to prevent cicular references when serialzing to JSON

namespace ExpenseTracker.Domain.Entities; //keep it inside the same namespace as the other entities.That is why expense and account can easily see each other without using extra using statements

public enum CategoryType { Expense = 0, Income = 1 } //a small fixed list of allowed numbers 0 spend 1 earn

public class Category //This one entity defines what type of spending (or income) an expense belongs to, like food, rent etc
{  
    public int Id { get; set; } //primary key of the category table
    public string Name { get; set; } = default!; //food, groceries, rent etc
    public CategoryType Type { get; set; } = CategoryType.Expense; //links this category to the enum defined above, if i want later i can specify that i earn money with CategoryType.Income
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    [JsonIgnore] //each expense points to its own Category, no circular references
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>(); //one to many relationship, one category of expense can have many expenses itself, like food has lunch, groceries, coffee etc
     
}
