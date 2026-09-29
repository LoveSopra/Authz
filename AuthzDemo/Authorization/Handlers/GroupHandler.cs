using AuthzDemo.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace AuthzDemo.Authorization.Handlers;

/// <summary>Delas av både attribut- och IAuthorizationService-metoden.</summary>
public class GroupHandler : AuthorizationHandler<GroupRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupRequirement requirement)
    {
        if (context.User.HasClaim(AppClaims.Group, requirement.Group))
            context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
