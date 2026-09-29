using AuthzDemo.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace AuthzDemo.Authorization.Handlers;

/// <summary>Delas av både attribut- och IAuthorizationService-metoden.</summary>
public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.HasClaim(AppClaims.Permission, requirement.Permission))
            context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
