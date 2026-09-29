using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace AuthzDemo.Authorization.Requirements;

/// <summary>Operationer som kan utföras på ett dokument (resursbaserad auktorisering).</summary>
public static class DocumentOperations
{
    public static readonly OperationAuthorizationRequirement Edit = new() { Name = nameof(Edit) };
}
