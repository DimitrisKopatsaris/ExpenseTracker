using AutoMapper;
using ExpenseTracker.Application.DTOs.Categories;
using ExpenseTracker.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public CategoriesController(ICategoryService categoryService, IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
        var dtos = _mapper.Map<List<CategoryDto>>(categories);
        return Ok(dtos);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create(CreateCategoryDto dto)
    {
        var category = await _categoryService.CreateAsync(dto.Name, dto.Type);
        var result = _mapper.Map<CategoryDto>(category);

        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }
}
