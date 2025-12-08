using System.ComponentModel.DataAnnotations;
using Adoction.Domains.Enums;
using Adoction.Domains.Models;

namespace Adoction.Application.DTOs;

public record LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = default!;

    [Required]
    public string Password { get; init; } = default!;
}

public record GoogleLoginRequest
{
    [Required]
    public string IdToken { get; init; } = default!;
}

public record TokenResponse
{
    public string AccessToken { get; init; } = default!;
    public DateTime ExpiresAt { get; init; }
    public RoleType Role { get; init; }
    public IReadOnlyCollection<Permission> Permissions { get; init; } = Array.Empty<Permission>();
    public string Provider { get; init; } = "local";
}
