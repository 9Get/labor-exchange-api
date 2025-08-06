using AutoMapper;
using LaborExchangeApi.Data;
using LaborExchangeApi.Dtos.UnemployedDtos;
using LaborExchangeApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaborExchangeApi.Controllers;

[Route("api/unemployed")]
[ApiController]
public class UnemployedController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UnemployedController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UnemployedDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<UnemployedDto>>> SearchUnemployed([FromQuery] string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return BadRequest("Search keyword cannot be empty.");
        }

        var query = _context.Unemployed.AsQueryable();

        var searchTerm = keyword.ToLower().Trim();

        query = query.Where(u =>
            EF.Functions.ILike(u.FirstName, $"%{searchTerm}%") ||
            EF.Functions.ILike(u.LastName, $"%{searchTerm}%")
        );

        var unemployed = await query
            .Include(u => u.Resume)
            .ToListAsync();

        var unemployedDto = _mapper.Map<IEnumerable<UnemployedDto>>(unemployed);

        return Ok(unemployedDto);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UnemployedDto>))]
    public async Task<ActionResult<IEnumerable<UnemployedDto>>> GetUnemployed([FromQuery] string? sortBy)
    {
        var query = _context.Unemployed.AsQueryable();

        if (!string.IsNullOrEmpty(sortBy))
        {
            if (sortBy.Equals("firstName", StringComparison.OrdinalIgnoreCase))
                query = query.OrderBy(u => u.FirstName);

            if (sortBy.Equals("lastName", StringComparison.OrdinalIgnoreCase))
                query = query.OrderBy(u => u.LastName);
        }

        var unemployed = await query
                .Include(u => u.Resume)
                .ToListAsync();

        var unemployedDto = _mapper.Map<IEnumerable<UnemployedDto>>(unemployed);

        return Ok(unemployedDto);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UnemployedDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UnemployedDto>> GetUnemployedById(int id)
    {
        var unemployed = await _context.Unemployed
            .Include(u => u.Resume)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (unemployed == null)
        {
            return NotFound();
        }

        var unemployedDto = _mapper.Map<UnemployedDto>(unemployed);

        return Ok(unemployedDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UnemployedDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UnemployedDto>> CreateUnemployed([FromBody] CreateUnemployedDto unemployedDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var newUnemployed = _mapper.Map<Unemployed>(unemployedDto);

        _context.Unemployed.Add(newUnemployed);
        await _context.SaveChangesAsync();

        var createdUnemployedWithIncludes = await _context.Unemployed
            .Include(u => u.Resume)
            .FirstOrDefaultAsync(u => u.Id == newUnemployed.Id);

        var resultDto = _mapper.Map<UnemployedDto>(createdUnemployedWithIncludes);

        return CreatedAtAction(nameof(GetUnemployedById), new { id = resultDto.Id }, resultDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUnemployed(int id, [FromBody] UpdateUnemployedDto unemployedDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var unemployedToUpdate = await _context.Unemployed.FindAsync(id);

        if (unemployedToUpdate == null)
        {
            return NotFound();
        }

        _mapper.Map(unemployedDto, unemployedToUpdate);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchUnemployed(int id, JsonPatchDocument<PatchUnemployedDto> patchDocument)
    {
        if (patchDocument == null)
        {
            return BadRequest();
        }

        var unemployedFromDb = await _context.Unemployed.FindAsync(id);
        if (unemployedFromDb == null)
        {
            return NotFound();
        }

        var unemployedToPatch = _mapper.Map<PatchUnemployedDto>(unemployedFromDb);

        patchDocument.ApplyTo(unemployedToPatch, ModelState);

        if (!TryValidateModel(unemployedToPatch))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(unemployedToPatch, unemployedFromDb);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUnemployed(int id)
    {
        var unemployed = await _context.Unemployed.FindAsync(id);

        if (unemployed == null)
        {
            return NotFound();
        }

        _context.Unemployed.Remove(unemployed);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}