# Authz-demo: attribut vs IAuthorizationService

Minimal API (.NET 10) som visar två sätt att auktorisera:

| | Attribut | IAuthorizationService |
|---|---|---|
| Stil | Deklarativt (`[Authorize(...)]`) | Imperativt i koden |
| Dynamiska värden (t.ex. gruppnamn från URL) | Nej, endast konstanter | Ja |
| Resursbaserat (t.ex. "är ägare till dokumentet") | Nej | Ja |
| Kräver registrerad policy | Ja (för allt utöver roller) | Nej |
| Återanvändbar logik | Via policy/handler | Via samma handlers |

## Kör

```
cd AuthzDemo
dotnet run
```

Öppna adressen som skrivs ut (t.ex. http://localhost:5000).

## Användare (lösenord `demo`)

| Användare | Roll | Grupper | Rättigheter |
|---|---|---|---|
| anna | Admin | Ekonomi, Forsaljning | read, write |
| bosse | User | Forsaljning | read |
| cecilia | User | Ekonomi | read, write |
| dan | User | – | – |

## Filer

```
AuthzDemo/
├─ Program.cs                          Startup, bara registrering och mappning
├─ Endpoints/
│  ├─ AuthEndpoints.cs                 Login / logout / me
│  ├─ AttributeEndpoints.cs            Metod 1: [Authorize]
│  └─ AuthzServiceEndpoints.cs         Metod 2: IAuthorizationService
├─ Authorization/
│  ├─ ServiceCollectionExtensions.cs   Registrerar autentisering, policies och handlers
│  ├─ AppClaims.cs / AppPermissions.cs / PolicyNames.cs
│  ├─ Requirements/                    GroupRequirement, PermissionRequirement, DocumentOperations
│  └─ Handlers/                        GroupHandler, PermissionHandler, DocumentEditHandler
├─ Models/                             DemoUser, Document, LoginRequest
├─ Data/                               UserStore, DocumentStore (i minnet)
└─ wwwroot/index.html                  Webbgränssnitt
```
