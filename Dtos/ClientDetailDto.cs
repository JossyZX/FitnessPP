using FitnessPP.Models;

namespace FitnessPP.Dtos;

public class ClientDetailDto
{
    public Guid Id { get; set; }
    public string Surname { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Patronymic { get; set; }
    public DateTime Birthday { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Trainer? Trainer { get; set; }
    public Locker? Locker { get; set; } // Добавили по ТЗ 5.1
    public List<Service> Services { get; set; } = new(); // Добавили по ТЗ 5.1
}
