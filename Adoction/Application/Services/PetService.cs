using Adoction.Application.DTOs;
using Adoction.Domains.Interfaces;
using Adoction.Domains.Models;

namespace Adoction.Application.Services;

public class PetService : IPetService
{
    private readonly IPetRepository _repository;

    public PetService(IPetRepository repository)
    {
        _repository = repository;
    }

    public async Task<Pet> CreateAsync(CreatePetRequest request, CancellationToken cancellationToken = default)
    {
        var pet = new Pet
        {
            Name = request.Name,
            Raza = request.Raza,
            Age = request.Age,
            Vacunado = request.Vacunado,
            NombreVacunas = request.NombreVacunas ?? new List<string>(),
            Esterilizado = request.Esterilizado,
            CertificadoPedigree = request.CertificadoPedigree,
            Size = request.Size,
            Genero = request.Genero,
            Species = request.Species,
            ShelterId = request.ShelterId
        };

        await _repository.AddAsync(pet, cancellationToken);
        return pet;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.DeleteAsync(id, cancellationToken);
    }

    public Task<Pet?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        return _repository.GetAsync(id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Pet>> SearchAsync(PetQuery query, CancellationToken cancellationToken = default)
    {
        var pets = await _repository.GetAllAsync(cancellationToken);

        return pets
            .Where(p => query.Status is null || p.Status == query.Status)
            .Where(p => query.Species is null || p.Species == query.Species)
            .Where(p => query.Gender is null || p.Genero == query.Gender)
            .Where(p => query.Size is null || p.Size == query.Size)
            .ToList();
    }

    public async Task<Pet?> UpdateAsync(int id, UpdatePetRequest request, CancellationToken cancellationToken = default)
    {
        var pet = await _repository.GetAsync(id, cancellationToken);
        if (pet is null)
        {
            return null;
        }

        pet.Name = request.Name;
        pet.Raza = request.Raza;
        pet.Age = request.Age;
        pet.Vacunado = request.Vacunado;
        pet.NombreVacunas = request.NombreVacunas ?? new List<string>();
        pet.Esterilizado = request.Esterilizado;
        pet.CertificadoPedigree = request.CertificadoPedigree;
        pet.Size = request.Size;
        pet.Genero = request.Genero;
        pet.Species = request.Species;
        pet.ShelterId = request.ShelterId;

        await _repository.UpdateAsync(pet, cancellationToken);
        return pet;
    }

    public async Task<Pet?> UpdateStatusAsync(int id, UpdatePetStatusRequest request, CancellationToken cancellationToken = default)
    {
        var pet = await _repository.GetAsync(id, cancellationToken);
        if (pet is null)
        {
            return null;
        }

        pet.Status = request.Status;
        await _repository.UpdateAsync(pet, cancellationToken);
        return pet;
    }
}
