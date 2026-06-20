using FitnessPP.Models;

namespace FitnessPP.Repositories;

public class InMemoryTrainerRepository : ITrainerRepository
{
    private readonly List<Trainer> _trainers = new();

    public List<Trainer> GetAll() => _trainers;

    public Trainer? GetById(Guid id) => _trainers.FirstOrDefault(t => t.Id == id);

    public void Add(Trainer trainer) => _trainers.Add(trainer);

    public void Update(Trainer trainer)
    {
        var existing = GetById(trainer.Id);
        if (existing != null)
        {
            existing.Surname = trainer.Surname;
            existing.Name = trainer.Name;
            existing.Patronymic = trainer.Patronymic;
            existing.Phone = trainer.Phone;
            existing.Status = trainer.Status;
        }
    }
}
