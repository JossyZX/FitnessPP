using FitnessPP.Dtos;
using FitnessPP.Models;
using FitnessPP.Repositories;

namespace FitnessPP.Services;

public class FitnessService
{
    private readonly IClientRepository _clientRepository;
    private readonly ITrainerRepository _trainerRepository;

    public FitnessService(IClientRepository clientRepository, ITrainerRepository trainerRepository)
    {
        _clientRepository = clientRepository;
        _trainerRepository = trainerRepository;
    }

    #region Trainer Methods

    public List<Trainer> GetAllTrainers() => _trainerRepository.GetAll();

    public Trainer? GetTrainerById(Guid id) => _trainerRepository.GetById(id);

    public Trainer CreateTrainer(Trainer trainer)
    {
        _trainerRepository.Add(trainer);
        return trainer;
    }

    public Trainer? UpdateTrainer(Guid id, Trainer updatedTrainer)
    {
        var existing = _trainerRepository.GetById(id);
        if (existing == null) return null;

        updatedTrainer.Id = id; // Гарантируем, что ID совпадает
        _trainerRepository.Update(updatedTrainer);
        return updatedTrainer;
    }

    public Trainer? UpdateTrainerStatus(Guid id, TrainerStatus newStatus)
    {
        var trainer = _trainerRepository.GetById(id);
        if (trainer == null) return null;

        trainer.Status = newStatus;
        _trainerRepository.Update(trainer);
        return trainer;
    }

    public TrainerDetailDto? GetTrainerDetail(Guid id)
    {
        var trainer = _trainerRepository.GetById(id);
        if (trainer == null) return null;

        var clients = _clientRepository.GetByTrainerId(id);

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

    public List<Client> GetAllClients() => _clientRepository.GetAll();

    public Client? GetClientById(Guid id) => _clientRepository.GetById(id);

    public Client CreateClient(Client client)
    {
        _clientRepository.Add(client);
        return client;
    }

    public Client? UpdateClient(Guid id, Client updatedClient)
    {
        var existing = _clientRepository.GetById(id);
        if (existing == null) return null;

        updatedClient.Id = id;
        updatedClient.TrainerId = existing.TrainerId;
        _clientRepository.Update(updatedClient);
        return updatedClient;
    }

    public Client? UpdateClientStatus(Guid id, bool isActive)
    {
        var client = _clientRepository.GetById(id);
        if (client == null) return null;

        client.IsActive = isActive;
        _clientRepository.Update(client);
        return client;
    }

    public string? AssignTrainer(Guid clientId, Guid trainerId)
    {
        var client = _clientRepository.GetById(clientId);
        if (client == null) return "ClientNotFound";

        var trainer = _trainerRepository.GetById(trainerId);
        if (trainer == null) return "TrainerNotFound";

        client.TrainerId = trainerId;
        _clientRepository.Update(client);
        return "Success";
    }

    public ClientDetailDto? GetClientDetail(Guid id)
    {
        var client = _clientRepository.GetById(id);
        if (client == null) return null;

        Trainer? trainer = null;
        if (client.TrainerId.HasValue)
        {
            trainer = _trainerRepository.GetById(client.TrainerId.Value);
        }

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
            Trainer = trainer
        };
    }

    #endregion
}
