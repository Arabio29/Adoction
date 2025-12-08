using Adoction.Application.DTOs;

namespace Adoction.Application.Services;

public interface IAuthService
{
    Task<TokenResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<TokenResponse?> LoginWithGoogleAsync(GoogleLoginRequest request, CancellationToken cancellationToken = default);
}
