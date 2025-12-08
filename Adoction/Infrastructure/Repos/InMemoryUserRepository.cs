using System.Collections.Concurrent;
using System.Threading;
using Adoction.Application.Auth;
using Adoction.Domains.Enums;
using Adoction.Domains.Interfaces;
using Adoction.Domains.Models;

namespace Adoction.Infrastructure.Repos;

public class InMemoryUserRepository : IUserRepository
{
    private readonly ConcurrentDictionary<int, User> _users = new();
    private int _currentId;

    public InMemoryUserRepository()
    {
        Seed();
    }

    public Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        var id = Interlocked.Increment(ref _currentId);
        user.Id = id;
        _users[id] = Clone(user);
        return Task.FromResult(user);
    }

    public Task<User?> GetByCredentialsAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = _users.Values.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && u.Password == password);
        return Task.FromResult(user is null ? null : Clone(user));
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = _users.Values.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(user is null ? null : Clone(user));
    }

    public Task<User?> GetByGoogleIdAsync(string googleId, CancellationToken cancellationToken = default)
    {
        var user = _users.Values.FirstOrDefault(u => string.Equals(u.GoogleId, googleId, StringComparison.Ordinal));
        return Task.FromResult(user is null ? null : Clone(user));
    }

    public Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_users.TryGetValue(id, out var user) ? Clone(user) : null);
    }

    private void Seed()
    {
        var admin = new User
        {
            Name = "Administrador",
            Email = "admin@adoccion.local",
            Phone = "555-0000",
            Password = "ChangeMe!",
            Role = RoleType.Admin,
            Permissions = new List<Permission>
            {
                new() { Name = PermissionConstants.PetsRead },
                new() { Name = PermissionConstants.PetsWrite },
                new() { Name = PermissionConstants.UsersRead }
            }
        };

        var shelterManager = new User
        {
            Name = "Shelter Manager",
            Email = "shelter@adoccion.local",
            Phone = "555-1111",
            Password = "Shelter123",
            Role = RoleType.ShelterManager,
            Permissions = new List<Permission>
            {
                new() { Name = PermissionConstants.PetsRead },
                new() { Name = PermissionConstants.PetsWrite }
            }
        };

        var volunteer = new User
        {
            Name = "Volunteer",
            Email = "volunteer@adoccion.local",
            Phone = "555-2222",
            Password = "Volunteer123",
            Role = RoleType.Volunteer,
            Permissions = new List<Permission>
            {
                new() { Name = PermissionConstants.PetsRead }
            }
        };

        var adopter = new User
        {
            Name = "Adopter",
            Email = "adopter@adoccion.local",
            Phone = "555-3333",
            Password = "Adopter123",
            Role = RoleType.Adopter,
            Permissions = new List<Permission>
            {
                new() { Name = PermissionConstants.PetsRead }
            },
            GoogleId = "demo-google-subject"
        };

        AddAsync(admin).GetAwaiter().GetResult();
        AddAsync(shelterManager).GetAwaiter().GetResult();
        AddAsync(volunteer).GetAwaiter().GetResult();
        AddAsync(adopter).GetAwaiter().GetResult();
    }

    private static User Clone(User user)
    {
        return new User
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            Phone = user.Phone,
            Password = user.Password,
            Role = user.Role,
            GoogleId = user.GoogleId,
            Permissions = user.Permissions.Select(p => new Permission { Name = p.Name, Description = p.Description }).ToList()
        };
    }
}
