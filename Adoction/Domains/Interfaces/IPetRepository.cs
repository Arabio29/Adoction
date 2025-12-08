using Adoction.Domains.Models;

namespace Adoction.Domains.Interfaces;

public interface IPetRepository
{
    Task<IReadOnlyCollection<Pet>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Pet?> GetAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(Pet pet, CancellationToken cancellationToken = default);
    Task UpdateAsync(Pet pet, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
