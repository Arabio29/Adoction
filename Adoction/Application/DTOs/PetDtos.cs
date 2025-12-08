using System.ComponentModel.DataAnnotations;
using Adoction.Domains.Enums;

namespace Adoction.Application.DTOs;

public record PetQuery
{
    public PetStatus? Status { get; init; }
    public PetSpecies? Species { get; init; }
    public Gender? Gender { get; init; }
    public Size? Size { get; init; }
}

public record CreatePetRequest
{
    [Required]
    [StringLength(120)]
    public string Name { get; init; } = default!;

    [Required]
    [StringLength(120)]
    public string Raza { get; init; } = default!;

    [Range(0, 40)]
    public int Age { get; init; }

    public bool Vacunado { get; init; }

    public List<string>? NombreVacunas { get; init; }

    public bool Esterilizado { get; init; }

    public bool CertificadoPedigree { get; init; }

    [Required]
    public Size Size { get; init; }

    [Required]
    public Gender Genero { get; init; }

    [Required]
    public PetSpecies Species { get; init; }

    [Range(1, int.MaxValue)]
    public int ShelterId { get; init; }
}

public record UpdatePetRequest : CreatePetRequest
{
    public new List<string>? NombreVacunas { get; init; }
}

public record UpdatePetStatusRequest
{
    [Required]
    public PetStatus Status { get; init; }
}

public record PetResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public string Raza { get; init; } = default!;
    public int Age { get; init; }
    public bool Vacunado { get; init; }
    public IReadOnlyCollection<string>? NombreVacunas { get; init; }
    public bool Esterilizado { get; init; }
    public bool CertificadoPedigree { get; init; }
    public Size Size { get; init; }
    public Gender Genero { get; init; }
    public PetStatus Status { get; init; }
    public PetSpecies Species { get; init; }
    public int ShelterId { get; init; }
}
