using AuthzDemo.Models;

namespace AuthzDemo.Data;

public static class DocumentStore
{
    public static readonly Document[] All =
    [
        new(1, "Budget 2026",       "cecilia"),
        new(2, "Säljrapport Q3",    "bosse"),
        new(3, "Styrelseprotokoll", "anna"),
        new(4, "Kundavtal Norden",  "erik"),
    ];

    public static Document? Find(int id) => All.FirstOrDefault(d => d.Id == id);
}
