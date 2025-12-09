using System.Collections.Generic;
using System.Linq;
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
        var pet = new Pet();

        ApplyPetDetails(pet, request);
        pet.Species = request.Species;
        pet.ShelterId = request.ShelterId;

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
        var criteria = new PetSearchCriteria
        {
            Status = query.Status,
            Species = query.Species,
            Gender = query.Gender,
            Size = query.Size
        };

        return await _repository.SearchAsync(criteria, cancellationToken);
    }

    public async Task<Pet?> UpdateAsync(int id, UpdatePetRequest request, CancellationToken cancellationToken = default)
    {
        var pet = await _repository.GetAsync(id, cancellationToken);
        if (pet is null)
        {
            return null;
        }

        ApplyPetDetails(pet, request);
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

    private static void ApplyPetDetails(Pet pet, CreatePetRequest request)
    {
        pet.Name = request.Name;
        pet.Raza = request.Raza;
        pet.Age = request.Age;
        pet.Vacunado = request.Vacunado;
        pet.NombreVacunas = NormalizeVaccination(request.Vacunado, request.NombreVacunas);
        pet.Esterilizado = request.Esterilizado;
        pet.CertificadoPedigree = request.CertificadoPedigree;
        pet.Size = request.Size;
        pet.Genero = request.Genero;
    }

    private static List<string> NormalizeVaccination(bool vacunado, List<string>? nombresVacunas)
    {
        if (!vacunado)
        {
            return new List<string>();
        }

        return (nombresVacunas ?? new List<string>())
            .Where(nombre => !string.IsNullOrWhiteSpace(nombre))
            .Select(nombre => nombre.Trim())
            .ToList();
    }
}
