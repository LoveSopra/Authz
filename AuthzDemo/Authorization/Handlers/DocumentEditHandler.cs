using AuthzDemo.Authorization.Requirements;
using AuthzDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace AuthzDemo.Authorization.Handlers;

/// <summary>
/// Resursbaserad auktorisering: beslutet beror på DET SPECIFIKA dokumentet.
/// Detta går INTE att uttrycka med [Authorize]-attribut, eftersom dokumentet
/// först hämtas inne i endpointen.
/// Regel: skrivrättighet krävs, och dessutom måste man vara ägare av dokumentet eller Admin.
/// </summary>
public class DocumentEditHandler : AuthorizationHandler<OperationAuthorizationRequirement, Document>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Document doc)
    {
        if (requirement != DocumentOperations.Edit) return Task.CompletedTask;

        var canWrite = context.User.HasClaim(AppClaims.Permission, AppPermissions.DocumentsWrite);
        var isOwner = context.User.Identity?.Name == doc.Owner;
        var isAdmin = context.User.IsInRole("Admin");

        if (canWrite && (isOwner || isAdmin))
            context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
