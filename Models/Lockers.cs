using System.Text.Json.Serialization;

namespace FitnessPP.Models;

public class Locker
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Number { get; set; }
    public Guid? ClientId { get; set; }

    [JsonIgnore]
    public Client? Client { get; set; }
}
