using System;
using System.Collections.Generic;
using System.Linq;
using ExpenseTracker.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseService _expenseService;

    public ExpensesController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    // GET: /api/expenses
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetAll()
    {
        var expenses = await _expenseService.GetAllWithDetailsAsync();

        var data = expenses
            .Select(e => new
            {
                e.Id,
                e.Amount,
                e.OccurredOnUtc,
                e.Note,
                Account = new { e.AccountId, e.Account.Name },
                Category = new { e.CategoryId, e.Category.Name, e.Category.Type }
            })
            .ToList();

        return Ok(data);
    }

    // GET: /api/expenses/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<object>> GetById(int id)
    {
        var e = await _expenseService.GetByIdWithDetailsAsync(id);

        if (e is null)
            return NotFound();

        var dto = new
        {
            e.Id,
            e.Amount,
            e.OccurredOnUtc,
            e.Note,
            Account = new { e.AccountId, e.Account.Name },
            Category = new { e.CategoryId, e.Category.Name, e.Category.Type }
        };

        return Ok(dto);
    }

    // GET: /api/expenses/by-account/{accountId}?from=2025-11-01&to=2025-11-30
    [HttpGet("by-account/{accountId:int}")]
    public async Task<ActionResult<IEnumerable<object>>> GetByAccount(
        int accountId, DateTime? from, DateTime? to)
    {
        var expenses = await _expenseService.GetByAccountAsync(accountId, from, to);

        var data = expenses
            .Select(e => new
            {
                e.Id,
                e.Amount,
                e.OccurredOnUtc,
                e.Note,
                Category = new { e.CategoryId, e.Category.Name, e.Category.Type }
            })
            .OrderByDescending(e => e.OccurredOnUtc)
            .ToList();

        return Ok(data);
    }

    // GET: /api/expenses/by-category/{categoryId}?from=2025-11-01&to=2025-11-30
    [HttpGet("by-category/{categoryId:int}")]
    public async Task<ActionResult<IEnumerable<object>>> GetByCategory(
        int categoryId, DateTime? from, DateTime? to)
    {
        var expenses = await _expenseService.GetByCategoryAsync(categoryId, from, to);

        var data = expenses
            .Select(e => new
            {
                e.Id,
                e.Amount,
                e.OccurredOnUtc,
                e.Note,
                Account = new { e.AccountId, e.Account.Name }
            })
            .OrderByDescending(e => e.OccurredOnUtc)
            .ToList();

        return Ok(data);
    }

    // GET: /api/expenses/range?from=2025-11-01&to=2025-11-30
    [HttpGet("range")]
    public async Task<ActionResult<IEnumerable<object>>> GetByDateRange(
        DateTime? from, DateTime? to)
    {
        var expenses = await _expenseService.GetByDateRangeAsync(from, to);

        var data = expenses
            .Select(e => new
            {
                e.Id,
                e.Amount,
                e.OccurredOnUtc,
                e.Note,
                Account = new { e.AccountId, e.Account.Name },
                Category = new { e.CategoryId, e.Category.Name, e.Category.Type }
            })
            .OrderByDescending(e => e.OccurredOnUtc)
            .ToList();

        return Ok(data);
    }

    public record CreateExpenseRequest(int AccountId, int CategoryId, decimal Amount, string? Note, DateTime OccurredOnUtc);

    // POST: /api/expenses
    [HttpPost]
    public async Task<ActionResult<object>> Create(CreateExpenseRequest req)
    {
        if (req.Amount <= 0)
            return ValidationProblem("Amount must be greater than 0.");

        try
        {
            var expense = await _expenseService.CreateAsync(
                req.AccountId,
                req.CategoryId,
                req.Amount,
                req.Note,
                req.OccurredOnUtc);

            var dto = new
            {
                expense.Id,
                expense.Amount,
                expense.OccurredOnUtc,
                expense.Note,
                Account = new { expense.AccountId },
                Category = new { expense.CategoryId }
            };

            return CreatedAtAction(nameof(GetById), new { id = expense.Id }, dto);
        }
        catch (InvalidOperationException ex)
        {
            // from the service when account/category is invalid
            return ValidationProblem(ex.Message);
        }
    }
}
