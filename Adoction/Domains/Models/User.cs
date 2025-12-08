using Adoction.Domains.Enums;

namespace Adoction.Domains.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Password { get; set; } = default!;
    public RoleType Role { get; set; }
    public string? GoogleId { get; set; }

    public List<Permission> Permissions { get; set; } = new();
}
