using FitnessPP.Models;

namespace FitnessPP.Repositories;

public interface ITrainerRepository
{
    List<Trainer> GetAll();
    Trainer? GetById(Guid id);
    void Add(Trainer trainer);
    void Update(Trainer trainer);
}
