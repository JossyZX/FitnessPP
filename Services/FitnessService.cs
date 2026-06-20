using FitnessPP.Dtos;
using FitnessPP.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessPP.Services;

public class FitnessService
{
    private readonly AppDbContext _context;

    public FitnessService(AppDbContext context)
    {
        _context = context;
    }

    #region Trainer Methods

    public List<Trainer> GetAllTrainers() => _context.Trainers.ToList();

    public Trainer? GetTrainerById(Guid id) => _context.Trainers.Find(id);

    public Trainer CreateTrainer(Trainer trainer)
    {
        _context.Trainers.Add(trainer);
        _context.SaveChanges();
        return trainer;
    }

    public Trainer? UpdateTrainer(Guid id, Trainer updatedTrainer)
    {
        var existing = _context.Trainers.Find(id);
        if (existing == null) return null;

        existing.Surname = updatedTrainer.Surname;
        existing.Name = updatedTrainer.Name;
        existing.Patronymic = updatedTrainer.Patronymic;
        existing.Phone = updatedTrainer.Phone;
        existing.Status = updatedTrainer.Status;

        _context.SaveChanges();
        return existing;
    }

    public Trainer? UpdateTrainerStatus(Guid id, TrainerStatus newStatus)
    {
        var trainer = _context.Trainers.Find(id);
        if (trainer == null) return null;

        trainer.Status = newStatus;
        _context.SaveChanges();
        return trainer;
    }

    public TrainerDetailDto? GetTrainerDetail(Guid id)
    {
        var trainer = _context.Trainers.Find(id);
        if (trainer == null) return null;

        var clients = _context.Clients.Where(c => c.TrainerId == id).ToList();

        return new TrainerDetailDto
        {
            Id = trainer.Id,
            Surname = trainer.Surname,
            Name = trainer.Name,
            Patronymic = trainer.Patronymic,
            Phone = trainer.Phone,
            Status = trainer.Status,
            Clients = clients
        };
    }

    #endregion

    #region Client Methods

    public List<Client> GetAllClients() => _context.Clients.ToList();

    public Client? GetClientById(Guid id) => _context.Clients.Find(id);

    public Client CreateClient(Client client)
    {
        _context.Clients.Add(client);
        _context.SaveChanges();
        return client;
    }

    public Client? UpdateClient(Guid id, Client updatedClient)
    {
        var existing = _context.Clients.Find(id);
        if (existing == null) return null;

        existing.Surname = updatedClient.Surname;
        existing.Name = updatedClient.Name;
        existing.Patronymic = updatedClient.Patronymic;
        existing.Birthday = updatedClient.Birthday;
        existing.Phone = updatedClient.Phone;
        existing.Email = updatedClient.Email;
        existing.IsActive = updatedClient.IsActive;

        _context.SaveChanges();
        return existing;
    }

    public Client? UpdateClientStatus(Guid id, bool isActive)
    {
        var client = _context.Clients.Find(id);
        if (client == null) return null;

        client.IsActive = isActive;
        _context.SaveChanges();
        return client;
    }

    public string? AssignTrainer(Guid clientId, Guid trainerId)
    {
        var client = _context.Clients.Find(clientId);
        if (client == null) return "ClientNotFound";

        var trainer = _context.Trainers.Find(trainerId);
        if (trainer == null) return "TrainerNotFound";

        client.TrainerId = trainerId;
        _context.SaveChanges();
        return "Success";
    }

    public ClientDetailDto? GetClientDetail(Guid id)
    {
        var client = _context.Clients
            .Include(c => c.Locker)
            .Include(c => c.Services)
            .FirstOrDefault(c => c.Id == id);

        if (client == null) return null;

        Trainer? trainer = client.TrainerId.HasValue ? _context.Trainers.Find(client.TrainerId.Value) : null;

        return new ClientDetailDto
        {
            Id = client.Id,
            Surname = client.Surname,
            Name = client.Name,
            Patronymic = client.Patronymic,
            Birthday = client.Birthday,
            Phone = client.Phone,
            Email = client.Email,
            IsActive = client.IsActive,
            Trainer = trainer,
            Locker = client.Locker,
            Services = client.Services
        };
    }

    #endregion

    #region Locker & Service Methods (Новая бизнес-логика ПП04)

    public List<Locker> GetAllLockers() => _context.Lockers.ToList();

    public List<Service> GetAllServices() => _context.Services.ToList();

    public ServiceDetailDto? GetServiceDetail(string id)
    {
        var service = _context.Services
            .Include(s => s.Clients)
            .FirstOrDefault(s => s.Id == id);

        if (service == null) return null;

        return new ServiceDetailDto
        {
            Id = service.Id,
            Name = service.Name,
            Price = service.Price,
            Clients = service.Clients
        };
    }

    public string AssignLocker(Guid clientId, Guid lockerId)
    {
        var client = _context.Clients.Include(c => c.Locker).FirstOrDefault(c => c.Id == clientId);
        if (client == null) return "ClientNotFound";

        var locker = _context.Lockers.Find(lockerId);
        if (locker == null) return "LockerNotFound";

        if (locker.ClientId.HasValue && locker.ClientId != clientId)
        {
            return "LockerOccupied";
        }

        if (client.LockerId.HasValue && client.LockerId != lockerId)
        {
            return "ClientAlreadyHasLocker";
        }

        client.LockerId = lockerId;
        locker.ClientId = clientId;

        _context.SaveChanges();
        return "Success";
    }

    public string AddServiceToClient(Guid clientId, string serviceId)
    {
        var client = _context.Clients.Include(c => c.Services).FirstOrDefault(c => c.Id == clientId);
        if (client == null) return "ClientNotFound";

        var service = _context.Services.Find(serviceId);
        if (service == null) return "ServiceNotFound";

        if (client.Services.Any(s => s.Id == serviceId))
        {
            return "AlreadySubscribed";
        }

        client.Services.Add(service);
        _context.SaveChanges();
        return "Success";
    }

    #endregion
}
