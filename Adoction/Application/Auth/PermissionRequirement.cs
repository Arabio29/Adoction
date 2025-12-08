using Microsoft.AspNetCore.Authorization;

namespace Adoction.Application.Auth;

public record PermissionRequirement(string Permission) : IAuthorizationRequirement;
