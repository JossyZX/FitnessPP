using FitnessPP.Models;

namespace FitnessPP.Dtos;

public class ServiceDetailDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Price { get; set; }
    public List<Client> Clients { get; set; } = new();
}
