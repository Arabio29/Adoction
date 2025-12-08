using Adoction.Application.DTOs;
using Adoction.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adoction.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<TokenResponse>> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var token = await _authService.LoginAsync(request, cancellationToken);
        if (token is null)
        {
            return Unauthorized();
        }

        return Ok(token);
    }

    [HttpPost("google")]
    [AllowAnonymous]
    public async Task<ActionResult<TokenResponse>> GoogleLoginAsync([FromBody] GoogleLoginRequest request, CancellationToken cancellationToken)
    {
        var token = await _authService.LoginWithGoogleAsync(request, cancellationToken);
        if (token is null)
        {
            return Unauthorized();
        }

        return Ok(token);
    }
}
