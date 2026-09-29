using AuthzDemo.Authorization;
using AuthzDemo.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDemoAuthentication();
builder.Services.AddDemoAuthorization();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();           // login / logout / me
app.MapAttributeEndpoints();      // metod 1: [Authorize]
app.MapAuthzServiceEndpoints();   // metod 2: IAuthorizationService

app.Run();
