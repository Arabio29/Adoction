using System.Collections.Concurrent;
using System.Threading;
using Adoction.Domains.Interfaces;
using Adoction.Domains.Models;

namespace Adoction.Infrastructure.Repos;

public class InMemoryPetRepository : IPetRepository
{
    private readonly ConcurrentDictionary<int, Pet> _pets = new();
    private int _currentId;

    public Task AddAsync(Pet pet, CancellationToken cancellationToken = default)
    {
        var id = Interlocked.Increment(ref _currentId);
        pet.Id = id;
        _pets[id] = Clone(pet);
        return Task.CompletedTask;
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_pets.TryRemove(id, out _));
    }

    public Task<IReadOnlyCollection<Pet>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var snapshot = _pets.Values.Select(Clone).ToArray();
        return Task.FromResult<IReadOnlyCollection<Pet>>(snapshot);
    }

    public Task<Pet?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        if (_pets.TryGetValue(id, out var pet))
        {
            return Task.FromResult<Pet?>(Clone(pet));
        }

        return Task.FromResult<Pet?>(null);
    }

    public Task UpdateAsync(Pet pet, CancellationToken cancellationToken = default)
    {
        _pets.AddOrUpdate(pet.Id, _ => Clone(pet), (_, _) => Clone(pet));
        return Task.CompletedTask;
    }

    private static Pet Clone(Pet pet)
    {
        return new Pet
        {
            Id = pet.Id,
            Name = pet.Name,
            Raza = pet.Raza,
            Age = pet.Age,
            Vacunado = pet.Vacunado,
            NombreVacunas = pet.NombreVacunas is null ? null : new List<string>(pet.NombreVacunas),
            Esterilizado = pet.Esterilizado,
            CertificadoPedigree = pet.CertificadoPedigree,
            Size = pet.Size,
            Genero = pet.Genero,
            Status = pet.Status,
            Species = pet.Species,
            ShelterId = pet.ShelterId,
            Shelter = pet.Shelter
        };
    }
}
