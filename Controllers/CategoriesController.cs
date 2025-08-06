using AutoMapper;
using LaborExchangeApi.Data;
using LaborExchangeApi.Dtos;
using LaborExchangeApi.Dtos.CategoryDtos;
using LaborExchangeApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaborExchangeApi.Controllers;

[Route("api/categories")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CategoriesController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoryDto>))]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        var categories = await _context.Categories.ToListAsync();

        var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

        return Ok(categoryDtos);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryDto>> GetCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        var categoryDto = _mapper.Map<CategoryDto>(category);

        return Ok(categoryDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CategoryDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CreateCategoryDto categoryDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(categoryDto.Name))
        {
            return BadRequest("Category name cannot be empty.");
        }

        var newCategory = _mapper.Map<Category>(categoryDto);

        _context.Categories.Add(newCategory);
        await _context.SaveChangesAsync();

        var resultDto = _mapper.Map<CategoryDto>(newCategory);

        return CreatedAtAction(nameof(GetCategory), new { id = resultDto.Id }, resultDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto categoryDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var categoryToUpdate = await _context.Categories.FindAsync(id);
        if (categoryToUpdate == null)
        {
            return NotFound();
        }

        _mapper.Map(categoryDto, categoryToUpdate);

        await _context.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
