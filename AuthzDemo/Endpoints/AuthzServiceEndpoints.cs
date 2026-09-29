using System.Security.Claims;
using AuthzDemo.Authorization;
using AuthzDemo.Authorization.Requirements;
using AuthzDemo.Data;
using Microsoft.AspNetCore.Authorization;

namespace AuthzDemo.Endpoints;

/// <summary>METOD 2: AUTHZ-TJÄNSTEN (IAuthorizationService) – imperativt i koden.</summary>
public static class AuthzServiceEndpoints
{
    public static void MapAuthzServiceEndpoints(this WebApplication app)
    {
        // RequireAuthorization() på hela gruppen täcker "måste vara inloggad" en gång för alla,
        // så varje endpoint slipper kolla user.Identity.IsAuthenticated själv.
        // Allt utöver det (roll, grupp, behörighet, ägarskap) avgörs fortfarande imperativt
        // med IAuthorizationService, inne i respektive endpoint.
        var authz = app.MapGroup("/authz").RequireAuthorization();

        authz.MapGet("/inloggad", (ClaimsPrincipal user) =>
            Results.Ok(new { message = $"Hej {user.Identity!.Name}, du är inloggad." }));

        // Roll – vanlig kod, inget attribut
        authz.MapGet("/admin", (ClaimsPrincipal user) =>
            user.IsInRole("Admin")
                ? Results.Ok(new { message = "Du har rollen Admin." })
                : Results.Forbid());

        // Grupp – gruppnamnet kommer från URL:en! Omöjligt med attribut (måste vara konstant).
        authz.MapGet("/grupp/{namn}", async (string namn, ClaimsPrincipal user, IAuthorizationService authorization) =>
        {
            var result = await authorization.AuthorizeAsync(user, resource: null, new GroupRequirement(namn));
            return result.Succeeded
                ? Results.Ok(new { message = $"Du är med i gruppen {namn}." })
                : Results.Forbid();
        });

        // Läsa ett dokument: räcker med läsrättighet
        authz.MapGet("/dokument/{id:int}", async (int id, ClaimsPrincipal user, IAuthorizationService authorization) =>
        {
            var doc = DocumentStore.Find(id);
            if (doc is null) return Results.NotFound();

            var result = await authorization.AuthorizeAsync(user, resource: null, new PermissionRequirement(AppPermissions.DocumentsRead));
            return result.Succeeded
                ? Results.Ok(new { message = $"Du fick läsa '{doc.Title}'." })
                : Results.Forbid();
        });

        // Skrivrättighet
        authz.MapPut("/dokument", async (ClaimsPrincipal user, IAuthorizationService authorization) =>
        {
            var result = await authorization.AuthorizeAsync(user, resource: null, new PermissionRequirement(AppPermissions.DocumentsWrite));
            return result.Succeeded
                ? Results.Ok(new { message = "Du har skrivrättighet (documents:write)." })
                : Results.Forbid();
        });

        // Resursbaserad: beslutet tas EFTER att dokumentet hämtats, med hela objektet som input.
        authz.MapPut("/dokument/{id:int}", async (int id, ClaimsPrincipal user, IAuthorizationService authorization) =>
        {
            var doc = DocumentStore.Find(id);
            if (doc is null) return Results.NotFound();

            var result = await authorization.AuthorizeAsync(user, doc, DocumentOperations.Edit);
            return result.Succeeded
                ? Results.Ok(new { message = $"Du fick redigera '{doc.Title}'." })
                : Results.Forbid();
        });
    }
}
