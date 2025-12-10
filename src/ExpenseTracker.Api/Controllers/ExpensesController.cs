using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Controllers;

[ApiController]  //enables automatic model binding/validation and better error responses
[Route("api/[controller]")] //route base becomes api/expenses because controller name is ExpensesController.
public class ExpensesController : ControllerBase       //lightweight base class for APIs, gives all the fundamental tools needed to build an API controller.
{
    private readonly ExpenseTrackerDbContext _db; //DI, asp.net creates the controller and injects my ExpenseTrackerDbContext.
    public ExpensesController(ExpenseTrackerDbContext db) => _db = db; // i store the context in _db, to use inside actions.

    // GET: /api/expenses
    [HttpGet] //maps to get/api/expenses
    public async Task<ActionResult<IEnumerable<object>>> GetAll(CancellationToken ct) //This is a public, asynchronous API endpoint method named GetAll that will eventually return an HTTP response containing a list of objects, and it supports cancellation if the request is aborted.
    {
        var data = await _db.Expenses //start a linq query against the expense table
            .AsNoTracking() //read only optimization
            .Include(e => e.Account)
            .Include(e => e.Category)
            .Select(e => new {                      //return only what i need 
                e.Id,
                e.Amount,
                e.OccurredOnUtc,
                e.Note,
                Account = new { e.AccountId, e.Account.Name },
                Category = new { e.CategoryId, e.Category.Name, e.Category.Type }
            })
            .ToListAsync(ct); //executes the sql query asynchronously and gets the results

        return Ok(data); //returns HTTP 200 with the JSON body that come from var data 
    }

    // GET: /api/expenses/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<object>> GetById(int id, CancellationToken ct)
    {
        var e = await _db.Expenses
            .AsNoTracking()
            .Include(x => x.Account)
            .Include(x => x.Category)
            .Where(x => x.Id == id)
            .Select(x => new
            {
                x.Id,
                x.Amount,
                x.OccurredOnUtc,
                x.Note,
                Account = new { x.AccountId, x.Account.Name },
                Category = new { x.CategoryId, x.Category.Name, x.Category.Type }
            })
            .FirstOrDefaultAsync(ct);

        return e is null ? NotFound() : Ok(e);
    }
    
    // GET: /api/expenses/by-account/{accountId}?from=2025-11-01&to=2025-11-30
    [HttpGet("by-account/{accountId:int}")]
    public async Task<ActionResult<IEnumerable<object>>> GetByAccount(
        int accountId, DateTime? from, DateTime? to, CancellationToken ct)
    {
        var query = _db.Expenses
            .AsNoTracking()
            .Where(e => e.AccountId == accountId);

        if (from.HasValue) query = query.Where(e => e.OccurredOnUtc >= from.Value);
        if (to.HasValue)   query = query.Where(e => e.OccurredOnUtc <= to.Value);

        var data = await query
            .Include(e => e.Category)
            .Select(e => new {
                e.Id,
                e.Amount,
                e.OccurredOnUtc,
                e.Note,
                Category = new { e.CategoryId, e.Category.Name, e.Category.Type }
            })
            .OrderByDescending(e => e.OccurredOnUtc)
            .ToListAsync(ct);

        return Ok(data);
    }

    // GET: /api/expenses/by-category/{categoryId}?from=2025-11-01&to=2025-11-30
    [HttpGet("by-category/{categoryId:int}")]
    public async Task<ActionResult<IEnumerable<object>>> GetByCategory(
        int categoryId, DateTime? from, DateTime? to, CancellationToken ct)
    {
        var query = _db.Expenses
            .AsNoTracking()
            .Where(e => e.CategoryId == categoryId);

        if (from.HasValue) query = query.Where(e => e.OccurredOnUtc >= from.Value);
        if (to.HasValue)   query = query.Where(e => e.OccurredOnUtc <= to.Value);

        var data = await query
            .Include(e => e.Account)
            .Select(e => new {
                e.Id,
                e.Amount,
                e.OccurredOnUtc,
                e.Note,
                Account = new { e.AccountId, e.Account.Name }
            })
            .OrderByDescending(e => e.OccurredOnUtc)
            .ToListAsync(ct);

        return Ok(data);
    }

    // GET: /api/expenses/range?from=2025-11-01&to=2025-11-30
    [HttpGet("range")]
    public async Task<ActionResult<IEnumerable<object>>> GetByDateRange(
        DateTime? from, DateTime? to, CancellationToken ct)
    {
        var query = _db.Expenses.AsNoTracking();

        if (from.HasValue) query = query.Where(e => e.OccurredOnUtc >= from.Value);
        if (to.HasValue)   query = query.Where(e => e.OccurredOnUtc <= to.Value);

        var data = await query
            .Include(e => e.Account)
            .Include(e => e.Category)
            .Select(e => new {
                e.Id,
                e.Amount,
                e.OccurredOnUtc,
                e.Note,
                Account  = new { e.AccountId,  e.Account.Name },
                Category = new { e.CategoryId, e.Category.Name, e.Category.Type }
            })
            .OrderByDescending(e => e.OccurredOnUtc)
            .ToListAsync(ct);

        return Ok(data);
    }



    public record CreateExpenseRequest(int AccountId, int CategoryId, decimal Amount, string? Note, DateTime OccurredOnUtc);

    // POST: /api/expenses
    [HttpPost]
    public async Task<ActionResult<object>> Create(CreateExpenseRequest req, CancellationToken ct)
    {
        if (req.Amount <= 0)
            return ValidationProblem("Amount must be greater than 0.");

        var accountExists  = await _db.Accounts.AnyAsync(a => a.Id == req.AccountId, ct);
        var categoryExists = await _db.Categories.AnyAsync(c => c.Id == req.CategoryId, ct);

        if (!accountExists || !categoryExists)
            return ValidationProblem("Invalid AccountId or CategoryId.");

        var expense = new Expense // construct the entity with the provided data
        {
            AccountId = req.AccountId,
            CategoryId = req.CategoryId,
            Amount = req.Amount,
            Note = req.Note,
            OccurredOnUtc = req.OccurredOnUtc
        };

        _db.Expenses.Add(expense); //mark it for insert
        await _db.SaveChangesAsync(ct); //send an insert to sql server, and the database generates the new id

        // Return a lean DTO and a proper Location header to GET-by-id
        var dto = new {
            expense.Id,
            expense.Amount,
            expense.OccurredOnUtc,
            expense.Note,
            Account  = new { expense.AccountId },
            Category = new { expense.CategoryId }
        };

        return CreatedAtAction(nameof(GetById), new { id = expense.Id }, dto);
    }
}
