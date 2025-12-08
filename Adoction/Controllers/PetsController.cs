using Adoction.Application.Auth;
using Adoction.Application.DTOs;
using Adoction.Application.Services;
using Adoction.Domains.Models;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Policy = PermissionConstants.PetsRead)]
    public async Task<ActionResult<IEnumerable<PetResponse>>> GetAsync([FromQuery] PetQuery query, CancellationToken cancellationToken)
    {
        var pets = await _petService.SearchAsync(query, cancellationToken);
        return Ok(pets.Select(MapToResponse));
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = PermissionConstants.PetsRead)]
    public async Task<ActionResult<PetResponse>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var pet = await _petService.GetAsync(id, cancellationToken);
        if (pet is null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(pet));
    }

    [HttpPost]
    [Authorize(Policy = PermissionConstants.PetsWrite)]
    public async Task<ActionResult<PetResponse>> CreateAsync([FromBody] CreatePetRequest request, CancellationToken cancellationToken)
    {
        var pet = await _petService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = pet.Id }, MapToResponse(pet));
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = PermissionConstants.PetsWrite)]
    public async Task<ActionResult<PetResponse>> UpdateAsync(int id, [FromBody] UpdatePetRequest request, CancellationToken cancellationToken)
    {
        var pet = await _petService.UpdateAsync(id, request, cancellationToken);
        if (pet is null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(pet));
    }

    [HttpPatch("{id:int}/status")]
    [Authorize(Policy = PermissionConstants.PetsWrite)]
    public async Task<ActionResult<PetResponse>> UpdateStatusAsync(int id, [FromBody] UpdatePetStatusRequest request, CancellationToken cancellationToken)
    {
        var pet = await _petService.UpdateStatusAsync(id, request, cancellationToken);
        if (pet is null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(pet));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = PermissionConstants.PetsWrite)]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var deleted = await _petService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    private static PetResponse MapToResponse(Pet pet)
    {
        return new PetResponse
        {
            Id = pet.Id,
            Name = pet.Name,
            Raza = pet.Raza,
            Age = pet.Age,
            Vacunado = pet.Vacunado,
            NombreVacunas = pet.NombreVacunas,
            Esterilizado = pet.Esterilizado,
            CertificadoPedigree = pet.CertificadoPedigree,
            Size = pet.Size,
            Genero = pet.Genero,
            Status = pet.Status,
            Species = pet.Species,
            ShelterId = pet.ShelterId
        };
    }
}
