using AuthzDemo.Authorization.Handlers;
using AuthzDemo.Authorization.Requirements;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace AuthzDemo.Authorization;

public static class ServiceCollectionExtensions
{
    /// <summary>Cookie-autentisering som svarar 401/403 istället för redirect (API-vänligt).</summary>
    public static IServiceCollection AddDemoAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(o =>
            {
                o.Cookie.Name = "authz-demo";
                o.Events.OnRedirectToLogin = ctx => { ctx.Response.StatusCode = 401; return Task.CompletedTask; };
                o.Events.OnRedirectToAccessDenied = ctx => { ctx.Response.StatusCode = 403; return Task.CompletedTask; };
            });
        return services;
    }

    /// <summary>
    /// Named policies behövs BARA för attributmetoden ([Authorize(Policy = "...")]).
    /// Den imperativa metoden behöver dem inte – requirements skapas direkt i koden.
    /// Handlers delas av båda metoderna.
    /// </summary>
    public static IServiceCollection AddDemoAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(PolicyNames.Forsaljning, p => p.AddRequirements(new GroupRequirement("Forsaljning")))
            .AddPolicy(PolicyNames.Ekonomi,     p => p.AddRequirements(new GroupRequirement("Ekonomi")))
            .AddPolicy(PolicyNames.KanLasa,     p => p.AddRequirements(new PermissionRequirement(AppPermissions.DocumentsRead)))
            .AddPolicy(PolicyNames.KanSkriva,  p => p.AddRequirements(new PermissionRequirement(AppPermissions.DocumentsWrite)));

        services.AddSingleton<IAuthorizationHandler, GroupHandler>();
        services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
        services.AddSingleton<IAuthorizationHandler, DocumentEditHandler>();
        return services;
    }
}
