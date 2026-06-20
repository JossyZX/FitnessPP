using System.ComponentModel.DataAnnotations;

namespace FitnessPP.Models;

public class Trainer
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "Фамилия обязательна")]
    public string Surname { get; set; } = string.Empty;

    [Required(ErrorMessage = "Имя обязательно")]
    public string Name { get; set; } = string.Empty;

    public string? Patronymic { get; set; }

    [Required(ErrorMessage = "Номер телефона обязателен")]
    public string Phone { get; set; } = string.Empty;

    public TrainerStatus Status { get; set; } = TrainerStatus.WORKING;
}