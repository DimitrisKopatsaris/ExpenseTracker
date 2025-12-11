using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
        return Ok(categories);
    }

    public record CreateCategoryRequest(string Name, CategoryType Type);

    [HttpPost]
    public async Task<ActionResult<Category>> Create(CreateCategoryRequest req)
    {
        var cat = await _categoryService.CreateAsync(req.Name, req.Type);

        return CreatedAtAction(nameof(GetAll), new { id = cat.Id }, cat);
    }
}
