using AutoMapper;
using LaborExchangeApi.Data;
using LaborExchangeApi.Dtos;
using LaborExchangeApi.Dtos.ResumeDtos;
using LaborExchangeApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaborExchangeApi.Controllers;

[Route("api/resumes")]
[ApiController]
public class ResumesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ResumesController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ResumeDto>))]
    public async Task<ActionResult<IEnumerable<ResumeDto>>> GetResumes([FromQuery] string? sortBy)
    {
        var query = _context.Resumes
                            .Include(r => r.Category)
                            .Include(r => r.Unemployed)
                            .AsQueryable();

        if (!string.IsNullOrEmpty(sortBy))
        {
            if (sortBy.Equals("title", StringComparison.OrdinalIgnoreCase))
                query = query.OrderBy(r => r.Title);

            if (sortBy.Equals("experience", StringComparison.OrdinalIgnoreCase))
                query = query.OrderBy(r => r.Experience);
                
            if (sortBy.Equals("createdAt", StringComparison.OrdinalIgnoreCase))
                query = query.OrderBy(r => r.CreatedAt);
        }

        var resumes = await query.ToListAsync();

        var resumeDtos = _mapper.Map<IEnumerable<ResumeDto>>(resumes);

        return Ok(resumeDtos);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResumeDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ResumeDto>> GetResume(int id)
    {
        var resume = await _context.Resumes
            .Include(r => r.Category)
            .Include(r => r.Unemployed)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (resume == null)
        {
            return NotFound();
        }

        var resumeDto = _mapper.Map<ResumeDto>(resume);

        return Ok(resumeDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResumeDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ResumeDto>> CreateResume([FromBody] CreateResumeDto resumeDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var unemployedExists = await _context.Unemployed.AnyAsync(u => u.Id == resumeDto.UnemployedId);
        if (!unemployedExists)
        {
            return BadRequest($"Unemployed person with ID {resumeDto.UnemployedId} not found.");
        }

        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == resumeDto.CategoryId);
        if (!categoryExists)
        {
            return BadRequest($"Category with Id {resumeDto.CategoryId} not found");
        }

        var newResume = _mapper.Map<Resume>(resumeDto);

        _context.Resumes.Add(newResume);
        await _context.SaveChangesAsync();

        var createdResumeWithIncludes = await _context.Resumes
            .Include(r => r.Category)
            .Include(r => r.Unemployed)
            .FirstOrDefaultAsync(r => r.Id == newResume.Id);

        var resultDto = _mapper.Map<ResumeDto>(createdResumeWithIncludes);         

        return CreatedAtAction(nameof(GetResume), new { id = resultDto.Id }, resultDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateResume(int id, [FromBody] UpdateResumeDto resumeDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var resumeToUpdate = await _context.Resumes
            .Include(r => r.Category)
            .Include(r => r.Unemployed)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (resumeToUpdate == null)
        {
            return NotFound();
        }

        _mapper.Map(resumeDto, resumeToUpdate);

        await _context.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchResume(int id, JsonPatchDocument<PatchResumeDto> patchDocument)
    {
        if (patchDocument == null)
        {
            return BadRequest();
        }

        var resumeFromDb = await _context.Resumes.FindAsync(id);
        if (resumeFromDb == null)
        {
            return NotFound();
        }

        var resumeToPatch = _mapper.Map<PatchResumeDto>(resumeFromDb);

        patchDocument.ApplyTo(resumeToPatch, ModelState);

        if (!TryValidateModel(resumeToPatch))
        {
            return ValidationProblem(ModelState);
        }

        if (resumeToPatch.CategoryId != resumeFromDb.CategoryId)
        {
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == resumeToPatch.CategoryId);
            if (!categoryExists)
            {
                return BadRequest($"Category with ID {resumeToPatch.CategoryId} not found.");
            }
        }

        _mapper.Map(resumeToPatch, resumeFromDb);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteResume(int id)
    {
        var resume = await _context.Resumes.FindAsync(id);

        if (resume == null)
        {
            return NotFound();
        }

        _context.Resumes.Remove(resume);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}