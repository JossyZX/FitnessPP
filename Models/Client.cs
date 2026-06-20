using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FitnessPP.Models;

public class Client
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "Фамилия обязательна")]
    public string Surname { get; set; } = string.Empty;

    [Required(ErrorMessage = "Имя обязательно")]
    public string Name { get; set; } = string.Empty;

    public string? Patronymic { get; set; }

    [Required(ErrorMessage = "Дата рождения обязательна")]
    public DateTime Birthday { get; set; }

    [Required(ErrorMessage = "Номер телефона обязателен")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный формат Email")]
    public string Email { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public Guid? TrainerId { get; set; }

    public Guid? LockerId { get; set; }

    [JsonIgnore]
    public Locker? Locker { get; set; }

    [JsonIgnore]
    public List<Service> Services { get; set; } = new();
}
