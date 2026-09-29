using System.Security.Claims;
using AuthzDemo.Authorization;
using AuthzDemo.Data;
using AuthzDemo.Models;
using Microsoft.AspNetCore.Authentication;

namespace AuthzDemo.Endpoints;

/// <summary>Login / logout / vem är jag.</summary>
public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("/api");

        api.MapPost("/login", async (LoginRequest req, HttpContext http) =>
        {
            var user = UserStore.Find(req.UserName, req.Password);
            if (user is null) return Results.Json(new { error = "Fel användarnamn eller lösenord" }, statusCode: 401);

            var principal = user.ToPrincipal();
            await http.SignInAsync(principal);
            return Results.Ok(Describe(principal));
        });

        api.MapPost("/logout", async (HttpContext http) =>
        {
            await http.SignOutAsync();
            return Results.Ok();
        });

        api.MapGet("/me", (ClaimsPrincipal user) => Results.Ok(Describe(user)));

        api.MapGet("/users", () => UserStore.All.Select(u => new { u.UserName, u.DisplayName }));
    }

    private static object Describe(ClaimsPrincipal user) => new
    {
        isAuthenticated = user.Identity?.IsAuthenticated == true,
        userName = user.Identity?.Name,
        displayName = user.FindFirstValue(AppClaims.DisplayName),
        role = user.FindFirstValue(ClaimTypes.Role),
        groups = user.FindAll(AppClaims.Group).Select(c => c.Value),
        permissions = user.FindAll(AppClaims.Permission).Select(c => c.Value),
    };
}
