using AuthzDemo.Authorization;
using AuthzDemo.Models;

namespace AuthzDemo.Data;

public static class UserStore
{
    public static readonly DemoUser[] All =
    [
        new("anna",    "demo", "Anna Admin",      "Admin", ["Ekonomi", "Forsaljning"], [AppPermissions.DocumentsRead, AppPermissions.DocumentsWrite]),
        new("bosse",   "demo", "Bosse Säljare",   "User",  ["Forsaljning"],            [AppPermissions.DocumentsRead]),
        new("erik",    "demo", "Erik Säljare",    "User",  ["Forsaljning"],            [AppPermissions.DocumentsRead, AppPermissions.DocumentsWrite]),
        new("cecilia", "demo", "Cecilia Ekonomi", "User",  ["Ekonomi"],                [AppPermissions.DocumentsRead, AppPermissions.DocumentsWrite]),
        new("dan",     "demo", "Dan Praktikant",  "User",  [],                         []),
    ];

    public static DemoUser? Find(string userName, string password) =>
        All.FirstOrDefault(u =>
            u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase) && u.Password == password);
}
