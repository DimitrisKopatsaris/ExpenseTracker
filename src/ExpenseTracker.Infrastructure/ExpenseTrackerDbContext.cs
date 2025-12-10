using ExpenseTracker.Domain.Entities; //brings account, category, expense into scope so EF can map them.
using Microsoft.EntityFrameworkCore; //imports EF Core dbContext etc

namespace ExpenseTracker.Infrastructure;

public class ExpenseTrackerDbContext : DbContext
{
    public ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options) : base(options) { }

    public DbSet<Account> Accounts => Set<Account>(); 
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Expense> Expenses => Set<Expense>(); //these represent tables.Querying db.Expenses build SQL against the Expenses table.

    protected override void OnModelCreating(ModelBuilder modelBuilder) //mapping to C# classes to SQL
    {
        modelBuilder.Entity<Account>(b =>
        {
            b.ToTable("Accounts"); //table name = Accounts
            b.HasKey(a => a.Id); //primary key of the account
            b.Property(a => a.Name).IsRequired().HasMaxLength(200); // not null
            b.Property(a => a.StartingBalance).HasColumnType("decimal(18,2)"); //starting money of the account and max 18 numbers before the decimal point and 2 after the decimnal point.
        });

        modelBuilder.Entity<Category>(b =>
        {
            b.ToTable("Categories");
            b.HasKey(c => c.Id);
            b.Property(c => c.Name).IsRequired().HasMaxLength(200);
            b.Property(c => c.Type).IsRequired(); //this property cannot be null in the database, 0 for expense and 1 for income.
        });

        modelBuilder.Entity<Expense>(b =>
        {
            b.ToTable("Expenses");
            b.HasKey(e => e.Id);
            b.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            b.Property(e => e.Note).HasMaxLength(1000);
            
            // defines the relationships
            b.HasOne(e => e.Account) 
                .WithMany(a => a.Expenses)
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Restrict); // you cannot delete an account if expenses exist, prevents data loss.

            b.HasOne(e => e.Category)
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
