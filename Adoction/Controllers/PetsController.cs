using Adoction.Application.DTOs;
using Adoction.Application.Mappers;
using Adoction.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Adoction.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PetsController : ControllerBase
{
    private readonly IPetService _petService;

    public PetsController(IPetService petService)
    {
        _petService = petService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PetResponse>>> GetAsync([FromQuery] PetQuery query, CancellationToken cancellationToken)
    {
        var pets = await _petService.SearchAsync(query, cancellationToken);
        return Ok(pets.Select(PetMapper.ToResponse));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PetResponse>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var pet = await _petService.GetAsync(id, cancellationToken);
        if (pet is null)
        {
            return NotFound();
        }

        return Ok(pet.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<PetResponse>> CreateAsync([FromBody] CreatePetRequest request, CancellationToken cancellationToken)
    {
        var pet = await _petService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = pet.Id }, pet.ToResponse());
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PetResponse>> UpdateAsync(int id, [FromBody] UpdatePetRequest request, CancellationToken cancellationToken)
    {
        var pet = await _petService.UpdateAsync(id, request, cancellationToken);
        if (pet is null)
        {
            return NotFound();
        }

        return Ok(pet.ToResponse());
    }

    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<PetResponse>> UpdateStatusAsync(int id, [FromBody] UpdatePetStatusRequest request, CancellationToken cancellationToken)
    {
        var pet = await _petService.UpdateStatusAsync(id, request, cancellationToken);
        if (pet is null)
        {
            return NotFound();
        }

        return Ok(pet.ToResponse());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var deleted = await _petService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
