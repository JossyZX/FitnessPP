using FitnessPP.Models;

namespace FitnessPP.Repositories;

public interface IClientRepository
{
    List<Client> GetAll();
    Client? GetById(Guid id);
    List<Client> GetByTrainerId(Guid trainerId);
    void Add(Client client);
    void Update(Client client);
}
