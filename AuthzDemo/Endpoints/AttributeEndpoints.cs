using System.Security.Claims;
using AuthzDemo.Authorization;
using AuthzDemo.Data;
using Microsoft.AspNetCore.Authorization;

namespace AuthzDemo.Endpoints;

/// <summary>METOD 1: ATTRIBUT – deklarativt, bestäms vid kompilering.</summary>
public static class AttributeEndpoints
{
    public static void MapAttributeEndpoints(this WebApplication app)
    {
        var attr = app.MapGroup("/attr");

        // Måste vara inloggad
        attr.MapGet("/inloggad", [Authorize] (ClaimsPrincipal user) =>
            Results.Ok(new { message = $"Hej {user.Identity!.Name}, du är inloggad." }));

        // Måste ha rollen Admin
        attr.MapGet("/admin", [Authorize(Roles = "Admin")] () =>
            Results.Ok(new { message = "Du har rollen Admin." }));

        // Måste vara i gruppen Försäljning (gruppnamnet är hårdkodat i policyn)
        attr.MapGet("/grupp/forsaljning", [Authorize(Policy = PolicyNames.Forsaljning)] () =>
            Results.Ok(new { message = "Du är med i gruppen Försäljning." }));

        // Läsa ett dokument: räcker med läsrättighet
        attr.MapGet("/dokument/{id:int}", [Authorize(Policy = PolicyNames.KanLasa)] (int id) =>
        {
            var doc = DocumentStore.Find(id);
            return doc is null
                ? Results.NotFound()
                : Results.Ok(new { message = $"Du fick läsa '{doc.Title}'." });
        });

        // Måste ha skrivrättighet
        attr.MapPut("/dokument", [Authorize(Policy = PolicyNames.KanSkriva)] () =>
            Results.Ok(new { message = "Du har skrivrättighet (documents:write)." }));

        // Redigera specifikt dokument: attribut kan inte kolla ÄGARE –
        // vi kan bara kolla skrivrättighet, sen får vi göra manuella if-satser.
        attr.MapPut("/dokument/{id:int}", [Authorize(Policy = PolicyNames.KanSkriva)] (int id, ClaimsPrincipal user) =>
        {
            var doc = DocumentStore.Find(id);
            if (doc is null) return Results.NotFound();

            // Manuell, duplicerad logik som lätt glöms bort / hamnar i osynk med policyerna:
            if (doc.Owner != user.Identity!.Name && !user.IsInRole("Admin")) return Results.Forbid();

            return Results.Ok(new { message = $"Du fick redigera '{doc.Title}'." });
        });
    }
}
