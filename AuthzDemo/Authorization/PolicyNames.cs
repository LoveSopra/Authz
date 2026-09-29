namespace AuthzDemo.Authorization;

/// <summary>Policynamn – behövs bara för attributmetoden ([Authorize(Policy = ...)]).</summary>
public static class PolicyNames
{
    public const string Forsaljning = "Forsaljning";
    public const string Ekonomi = "Ekonomi";
    public const string KanLasa = "KanLasa";
    public const string KanSkriva = "KanSkriva";
}
