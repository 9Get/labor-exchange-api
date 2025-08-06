using AutoMapper;
using LaborExchangeApi.Data;
using LaborExchangeApi.Dtos;
using LaborExchangeApi.Dtos.CompanyDtos;
using LaborExchangeApi.Dtos.VacancyDtos;
using LaborExchangeApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaborExchangeApi.Controllers;

[Route("api/vacancies")]
[ApiController]
public class VacanciesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public VacanciesController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VacancyDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<VacancyDto>>> SearchVacancies([FromQuery] string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return BadRequest("Search keyword cannot be empty.");
        }

        var query = _context.Vacancies.AsQueryable();

        var searchTerm = keyword.ToLower().Trim();

        query = query.Where(v =>
            EF.Functions.ILike(v.Title, $"%{searchTerm}%") ||
            EF.Functions.ILike(v.Description, $"%{searchTerm}%")
        );

        var vacancies = await query
            .Include(v => v.Category)
            .Include(v => v.Company)
            .ToListAsync();

        var vacanciesDto = _mapper.Map<IEnumerable<VacancyDto>>(vacancies);

        return Ok(vacanciesDto);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VacancyDto>))]
    public async Task<ActionResult<IEnumerable<VacancyDto>>> GetVacancies([FromQuery] string? sortBy)
    {
        var query = _context.Vacancies
                            .Include(v => v.Company)
                            .Include(v => v.Category)
                            .AsQueryable();

        if (!string.IsNullOrEmpty(sortBy))
        {
            if (sortBy.Equals("title", StringComparison.OrdinalIgnoreCase))
                query = query.OrderBy(v => v.Title);

            if (sortBy.Equals("salary", StringComparison.OrdinalIgnoreCase))
                query = query.OrderBy(v => v.Salary);

            if (sortBy.Equals("createdAt", StringComparison.OrdinalIgnoreCase))
                query = query.OrderBy(v => v.CreatedAt);
        }

        var vacancies = await query.ToListAsync();

        var vacanciesDto = _mapper.Map<IEnumerable<VacancyDto>>(vacancies);

        return Ok(vacanciesDto);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VacancyDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VacancyDto>> GetVacancy(int id)
    {
        var vacancy = await _context.Vacancies
            .Include(v => v.Company)
            .Include(v => v.Category)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (vacancy == null)
        {
            return NotFound();
        }

        var vacancyDto = _mapper.Map<Vacancy>(vacancy);

        return Ok(vacancyDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(VacancyDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VacancyDto>> CreateVacancy([FromBody] CreateVacancyDto vacancyDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var companyExists = await _context.Companies.AnyAsync(c => c.Id == vacancyDto.CompanyId);
        if (!companyExists)
        {
            return BadRequest();

        }

        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == vacancyDto.CategoryId);
        if (!categoryExists)
        {
            return BadRequest();
        }

        var newVacancy = _mapper.Map<Vacancy>(vacancyDto);

        _context.Vacancies.Add(newVacancy);
        await _context.SaveChangesAsync();

        var createdVacancyWithIncludes = await _context.Vacancies
            .Include(v => v.Company)
            .Include(v => v.Category)
            .FirstOrDefaultAsync(v => v.Id == newVacancy.Id);

        var resultDto = _mapper.Map<VacancyDto>(createdVacancyWithIncludes);

        return CreatedAtAction(nameof(GetVacancy), new { id = resultDto.Id }, resultDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateVacancy(int id, [FromBody] UpdateVacancyDto vacancyDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var vacancyToUpdate = await _context.Vacancies
            .Include(v => v.Company)
            .Include(v => v.Category)
            .FirstOrDefaultAsync(v => v.Id == id);


        if (vacancyToUpdate == null)
        {
            return NotFound();
        }

        _mapper.Map(vacancyDto, vacancyToUpdate);
        
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchVacancy(int id, JsonPatchDocument<PatchVacancyDto> patchDocument)
    {
        if (patchDocument == null)
        {
            return BadRequest();
        }

        var vacancyFromDb = await _context.Vacancies.FindAsync(id);
        if (vacancyFromDb == null)
        {
            return NotFound();
        }

        var vacancyToPatch = _mapper.Map<PatchVacancyDto>(vacancyFromDb);

        patchDocument.ApplyTo(vacancyToPatch, ModelState);

        if (!TryValidateModel(vacancyToPatch))
        {
            return ValidationProblem(ModelState);
        }

        if (vacancyToPatch.CategoryId != vacancyFromDb.CategoryId)
        {
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == vacancyToPatch.CategoryId);
            if (!categoryExists)
            {
                return BadRequest($"Category with ID {vacancyToPatch.CategoryId} not found.");
            }
        }

        _mapper.Map(vacancyToPatch, vacancyFromDb);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteVacancy(int id)
    {
        var vacancy = await _context.Vacancies.FindAsync(id);

        if (vacancy == null)
        {
            return NotFound();
        }

        _context.Vacancies.Remove(vacancy);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}