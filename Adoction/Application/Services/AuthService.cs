using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Adoction.Application.Auth;
using Adoction.Application.DTOs;
using Adoction.Application.Options;
using Adoction.Domains.Enums;
using Adoction.Domains.Interfaces;
using Adoction.Domains.Models;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Adoction.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtOptions _jwtOptions;

    public AuthService(IUserRepository userRepository, IOptions<JwtOptions> jwtOptions)
    {
        _userRepository = userRepository;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<TokenResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByCredentialsAsync(request.Email, request.Password, cancellationToken);
        return user is null ? null : CreateToken(user, "local");
    }

    public async Task<TokenResponse?> LoginWithGoogleAsync(GoogleLoginRequest request, CancellationToken cancellationToken = default)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);
        var existingUser = await _userRepository.GetByGoogleIdAsync(payload.Subject, cancellationToken) ??
                          await _userRepository.GetByEmailAsync(payload.Email, cancellationToken);

        if (existingUser is null)
        {
            existingUser = await _userRepository.AddAsync(new User
            {
                Email = payload.Email,
                Name = payload.Name ?? payload.Email,
                GoogleId = payload.Subject,
                Phone = string.Empty,
                Password = string.Empty,
                Role = RoleType.Adopter,
                Permissions = new List<Permission> { new() { Name = PermissionConstants.PetsRead } }
            }, cancellationToken);
        }

        existingUser.GoogleId ??= payload.Subject;
        return CreateToken(existingUser, "google");
    }

    private TokenResponse CreateToken(User user, string provider)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.UniqueName, user.Name),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        claims.AddRange(user.Permissions.Select(permission => new Claim("perm", permission.Name)));

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        var handler = new JwtSecurityTokenHandler();
        return new TokenResponse
        {
            AccessToken = handler.WriteToken(token),
            ExpiresAt = expires,
            Role = user.Role,
            Permissions = user.Permissions,
            Provider = provider
        };
    }
}
