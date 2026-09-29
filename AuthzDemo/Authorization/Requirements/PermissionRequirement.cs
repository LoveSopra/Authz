using Microsoft.AspNetCore.Authorization;

namespace AuthzDemo.Authorization.Requirements;

/// <summary>Användaren ska ha en viss behörighet (t.ex. skrivrättighet).</summary>
public record PermissionRequirement(string Permission) : IAuthorizationRequirement;
