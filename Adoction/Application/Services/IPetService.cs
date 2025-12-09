using System.Threading;
using Adoction.Application.DTOs;
using Adoction.Domains.Models;

namespace Adoction.Application.Services;

public interface IPetService
{
    Task<IReadOnlyCollection<Pet>> SearchAsync(PetQuery query, CancellationToken cancellationToken = default);
    Task<Pet?> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<Pet> CreateAsync(CreatePetRequest request, CancellationToken cancellationToken = default);
    Task<Pet?> UpdateAsync(int id, UpdatePetRequest request, CancellationToken cancellationToken = default);
    Task<Pet?> UpdateStatusAsync(int id, UpdatePetStatusRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
