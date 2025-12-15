using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ExpenseTracker.Application.DTOs.Expenses;
using ExpenseTracker.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseService _expenseService;
    private readonly IMapper _mapper;

    public ExpensesController(IExpenseService expenseService, IMapper mapper)
    {
        _expenseService = expenseService;
        _mapper = mapper;
    }

    // GET: /api/expenses
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetAll()
    {
        var expenses = await _expenseService.GetAllAsync();
        var dtos = _mapper.Map<List<ExpenseDto>>(expenses);
        return Ok(dtos);
    }

    // GET: /api/expenses/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ExpenseDto>> GetById(int id)
    {
        var expense = await _expenseService.GetByIdAsync(id);

        if (expense is null)
            return NotFound();

        var dto = _mapper.Map<ExpenseDto>(expense);
        return Ok(dto);
    }

    // GET: /api/expenses/by-account/{accountId}?from=...&to=...
    [HttpGet("by-account/{accountId:int}")]
    public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetByAccount(
        int accountId, DateTime? from, DateTime? to)
    {
        var expenses = await _expenseService.GetByAccountAsync(accountId, from, to);
        var dtos = _mapper.Map<List<ExpenseDto>>(expenses);

        // keep the same ordering
        dtos = dtos.OrderByDescending(e => e.OccurredOnUtc).ToList();

        return Ok(dtos);
    }

    // GET: /api/expenses/by-category/{categoryId}?from=...&to=...
    [HttpGet("by-category/{categoryId:int}")]
    public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetByCategory(
        int categoryId, DateTime? from, DateTime? to)
    {
        var expenses = await _expenseService.GetByCategoryAsync(categoryId, from, to);
        var dtos = _mapper.Map<List<ExpenseDto>>(expenses);

        dtos = dtos.OrderByDescending(e => e.OccurredOnUtc).ToList();

        return Ok(dtos);
    }

    // GET: /api/expenses/range?from=...&to=...
    [HttpGet("range")]
    public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetByDateRange(
        DateTime? from, DateTime? to)
    {
        var expenses = await _expenseService.GetByDateRangeAsync(from, to);
        var dtos = _mapper.Map<List<ExpenseDto>>(expenses);

        dtos = dtos.OrderByDescending(e => e.OccurredOnUtc).ToList();

        return Ok(dtos);
    }

    // POST: /api/expenses
    [HttpPost]
    public async Task<ActionResult<ExpenseDto>> Create(CreateExpenseDto dto)
    {
        
        var created = await _expenseService.CreateAsync(
        dto.AccountId,
        dto.CategoryId,
        dto.Amount,
        dto.Note,
        dto.OccurredOnUtc);

        var result = _mapper.Map<ExpenseDto>(created);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, result);
       
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ExpenseDto>> Update(int id, UpdateExpenseDto dto)
    {
       
            var updated = await _expenseService.UpdateAsync(id,dto.AccountId,dto.CategoryId,dto.Amount,dto.Note,dto.OccurredOnUtc);
            if (updated is null) return NotFound();

            return Ok(_mapper.Map<ExpenseDto>(updated));
    
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _expenseService.DeleteAsync(id);

        if(!deleted) return NotFound();

        return NoContent();
    }
}
