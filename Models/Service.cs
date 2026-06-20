using System.Text.Json.Serialization;

namespace FitnessPP.Models;

public class Service
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Price { get; set; }

    [JsonIgnore]
    public List<Client> Clients { get; set; } = new();
}
