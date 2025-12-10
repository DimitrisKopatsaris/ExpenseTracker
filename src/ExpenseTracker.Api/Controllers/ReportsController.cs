using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly ExpenseTrackerDbContext _db;
    public ReportsController(ExpenseTrackerDbContext db) => _db = db;

    // GET: /api/reports/total-per-category?from=2025-11-01&to=2025-11-30
    [HttpGet("total-per-category")]
    public async Task<ActionResult<IEnumerable<object>>> GetTotalPerCategory(
        DateTime? from, DateTime? to, CancellationToken ct)
    {
        IQueryable<Expense> query = _db.Expenses.AsNoTracking();

        if (from.HasValue) query = query.Where(e => e.OccurredOnUtc >= from.Value);
        if (to.HasValue)   query = query.Where(e => e.OccurredOnUtc <= to.Value);

        var data = await query
            .GroupBy(e => new { e.CategoryId, e.Category.Name, e.Category.Type })
            .Select(g => new {
                g.Key.CategoryId,
                g.Key.Name,
                g.Key.Type,
                Total = g.Sum(x => x.Amount)
            })
            .OrderByDescending(x => x.Total)
            .ToListAsync(ct);

        return Ok(data);
        
    }

    // GET: /api/reports/account-balance
    // Balance = StartingBalance + sum(Income) - sum(Expense)
    [HttpGet("account-balance")]
    public async Task<ActionResult<IEnumerable<object>>> GetAccountBalances(CancellationToken ct)
    {
        var data = await _db.Accounts
            .AsNoTracking() //if i only read data there is no need to track, because i wont edit anything, no need to keep the object in memory and watch for changes. It would be usefull if i wanted to update something, then asnotraching wouldnt have a job in my code
            .Select(a => new
            {
                a.Id,
                a.Name,
                a.StartingBalance,
                Income  = a.Expenses
                    .Where(e => e.Category.Type == CategoryType.Income)
                    .Sum(e => (decimal?)e.Amount) ?? 0m,
                Expense = a.Expenses
                    .Where(e => e.Category.Type == CategoryType.Expense)
                    .Sum(e => (decimal?)e.Amount) ?? 0m
            })
            .Select(x => new {
                x.Id,
                x.Name,
                x.StartingBalance,
                Income  = x.Income,
                Expense = x.Expense,
                Balance = x.StartingBalance + x.Income - x.Expense
            })
            .OrderBy(x => x.Name)
            .ToListAsync(ct);

        return Ok(data);
    }
}
