using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ExpenseTrackerDbContext _db;
    public CategoriesController(ExpenseTrackerDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetAll()
        => Ok(await _db.Categories.AsNoTracking().ToListAsync());

    public record CreateCategoryRequest(string Name, CategoryType Type);

    [HttpPost]
    public async Task<ActionResult<Category>> Create(CreateCategoryRequest req)
    {
        var cat = new Category { Name = req.Name, Type = req.Type };
        _db.Categories.Add(cat);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = cat.Id }, cat);
    }
}

