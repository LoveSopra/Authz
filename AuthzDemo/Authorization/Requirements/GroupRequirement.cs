using Microsoft.AspNetCore.Authorization;

namespace AuthzDemo.Authorization.Requirements;

/// <summary>Användaren ska vara medlem i en viss grupp.</summary>
public record GroupRequirement(string Group) : IAuthorizationRequirement;
