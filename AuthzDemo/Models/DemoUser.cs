using System.Security.Claims;
using AuthzDemo.Authorization;

namespace AuthzDemo.Models;

/// <summary>Demoanvändare i minnet. I verkligheten: databas, Entra ID, AD-grupper osv.</summary>
public record DemoUser(string UserName, string Password, string DisplayName, string Role,
    string[] Groups, string[] Permissions)
{
    public ClaimsPrincipal ToPrincipal()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, UserName),
            new(ClaimTypes.Name, UserName),
            new(AppClaims.DisplayName, DisplayName),
            new(ClaimTypes.Role, Role),
        };
        claims.AddRange(Groups.Select(g => new Claim(AppClaims.Group, g)));
        claims.AddRange(Permissions.Select(p => new Claim(AppClaims.Permission, p)));

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies"));
    }
}
