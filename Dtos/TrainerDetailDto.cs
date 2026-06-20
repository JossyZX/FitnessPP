using FitnessPP.Models;

namespace FitnessPP.Dtos;

public class TrainerDetailDto
{
    public Guid Id { get; set; }
    public string Surname { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Patronymic { get; set; }
    public string Phone { get; set; } = string.Empty;
    public TrainerStatus Status { get; set; }
    public List<Client> Clients { get; set; } = new();
}