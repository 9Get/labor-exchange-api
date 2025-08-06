using System.Runtime.InteropServices;
using AutoMapper;
using LaborExchangeApi.Data;
using LaborExchangeApi.Dtos;
using LaborExchangeApi.Dtos.CompanyDtos;
using LaborExchangeApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaborExchangeApi.Controllers;

[Route("api/companies")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CompaniesController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CompanyDto>))]
    public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies([FromQuery] string? sortBy)
    {
        var query = _context.Companies.AsQueryable();

        if (!string.IsNullOrEmpty(sortBy) &&
            sortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
        {
            query = query.OrderBy(c => c.Name);
        }

        var companies = await query
                .Include(c => c.Vacancies)
                .ToListAsync();

        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

        return Ok(companiesDto);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CompanyDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CompanyDto>> GetCompany(int id)
    {
        var company = await _context.Companies
            .Include(c => c.Vacancies)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (company == null)
        {
            return NotFound();
        }

        var companyDto = _mapper.Map<CompanyDto>(company);

        return Ok(companyDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CompanyDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CompanyDto>> CreateCompany([FromBody] CreateCompanyDto companyDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var newCompany = _mapper.Map<Company>(companyDto);

        _context.Companies.Add(newCompany);
        await _context.SaveChangesAsync();

        var createdCompanyWithIncludes = await _context.Companies
            .Include(c => c.Vacancies)
            .FirstOrDefaultAsync(c => c.Id == newCompany.Id);

        var resultDto = _mapper.Map<CompanyDto>(createdCompanyWithIncludes);

        return CreatedAtAction(nameof(GetCompany), new { id = resultDto.Id }, resultDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCompany(int id, [FromBody] UpdateCompanyDto companyDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var companyToUpdate = await _context.Companies.FindAsync(id);

        if (companyToUpdate == null)
        {
            return NotFound();
        }

        _mapper.Map(companyDto, companyToUpdate);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchCompany(int id, [FromBody] JsonPatchDocument<PatchCompanyDto> patchDocument)
    {
        if (patchDocument == null)
        {
            return BadRequest();
        }

        var companyFromDb = await _context.Companies.FindAsync(id);
        if (companyFromDb == null)
        {
            return NotFound();
        }

        var companyToPatch = _mapper.Map<PatchCompanyDto>(companyFromDb);

        patchDocument.ApplyTo(companyToPatch, ModelState);

        if (!TryValidateModel(companyToPatch))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(companyToPatch, companyFromDb);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCompany(int id)
    {
        var company = await _context.Companies.FindAsync(id);

        if (company == null)
        {
            return NotFound();
        }

        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}