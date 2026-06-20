using FitnessPP.Models;

namespace FitnessPP.Repositories;

public class InMemoryClientRepository : IClientRepository
{
    private readonly List<Client> _clients = new();

    public List<Client> GetAll() => _clients;

    public Client? GetById(Guid id) => _clients.FirstOrDefault(c => c.Id == id);

    public List<Client> GetByTrainerId(Guid trainerId) =>
        _clients.Where(c => c.TrainerId == trainerId).ToList();

    public void Add(Client client) => _clients.Add(client);

    public void Update(Client client)
    {
        var existing = GetById(client.Id);
        if (existing != null)
        {
            existing.Surname = client.Surname;
            existing.Name = client.Name;
            existing.Patronymic = client.Patronymic;
            existing.Birthday = client.Birthday;
            existing.Phone = client.Phone;
            existing.Email = client.Email;
            existing.IsActive = client.IsActive;
            existing.TrainerId = client.TrainerId;
        }
    }
}
