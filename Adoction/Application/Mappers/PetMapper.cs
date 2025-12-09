using Adoction.Application.DTOs;
using Adoction.Domains.Models;

namespace Adoction.Application.Mappers;

public static class PetMapper
{
    public static PetResponse ToResponse(this Pet pet)
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
