using Adoction.Domains.Models;

namespace Adoction.Domains.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByCredentialsAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<User?> GetByGoogleIdAsync(string googleId, CancellationToken cancellationToken = default);
    Task<User> AddAsync(User user, CancellationToken cancellationToken = default);
}
