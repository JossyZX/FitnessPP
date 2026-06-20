using FitnessPP;
using FitnessPP.Models;
using FitnessPP.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. НАСТРОЙКА БАЗЫ ДАННЫХ И СЕРВИСОВ (DI)

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=fitness.db"));

builder.Services.AddScoped<FitnessService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 2. МАРШРУТЫ API

var apiGroup = app.MapGroup("/api");

#region CLIENT ENDPOINTS

var clientsGroup = apiGroup.MapGroup("/clients");

clientsGroup.MapPost("/", (Client client, FitnessService service) =>
{
    if (string.IsNullOrWhiteSpace(client.Surname) || string.IsNullOrWhiteSpace(client.Name))
        return Results.BadRequest(new { error = "Фамилия и Имя обязательны." });
    if (string.IsNullOrWhiteSpace(client.Email) || !client.Email.Contains('@'))
        return Results.BadRequest(new { error = "Некорректный формат Email." });

    var created = service.CreateClient(client);
    return Results.Created($"/api/clients/{created.Id}", created);
});

clientsGroup.MapGet("/", (FitnessService service) => Results.Ok(service.GetAllClients()));

clientsGroup.MapGet("/{id:guid}", (Guid id, FitnessService service) =>
{
    var client = service.GetClientById(id);
    return client is not null ? Results.Ok(client) : Results.NotFound();
});

clientsGroup.MapGet("/{id:guid}/detail", (Guid id, FitnessService service) =>
{
    var detailDto = service.GetClientDetail(id);
    return detailDto is not null ? Results.Ok(detailDto) : Results.NotFound();
});

clientsGroup.MapPut("/{id:guid}", (Guid id, Client updatedClient, FitnessService service) =>
{
    var result = service.UpdateClient(id, updatedClient);
    return result is not null ? Results.Ok(result) : Results.NotFound();
});

clientsGroup.MapPatch("/{id:guid}/status", (Guid id, [FromBody] bool isActive, FitnessService service) =>
{
    var result = service.UpdateClientStatus(id, isActive);
    return result is not null ? Results.Ok(result) : Results.NotFound();
});

clientsGroup.MapPost("/{clientId:guid}/trainer/{trainerId:guid}", (Guid clientId, Guid trainerId, FitnessService service) =>
{
    var outcome = service.AssignTrainer(clientId, trainerId);
    return outcome switch
    {
        "Success" => Results.Ok(new { message = "Тренер успешно назначен клиенту." }),
        "ClientNotFound" => Results.NotFound(new { error = "Клиент не найден." }),
        "TrainerNotFound" => Results.BadRequest(new { error = "Тренер не найден." }),
        _ => Results.BadRequest()
    };
});

clientsGroup.MapPost("/{clientId:guid}/locker/{lockerId:guid}", (Guid clientId, Guid lockerId, FitnessService service) =>
{
    var outcome = service.AssignLocker(clientId, lockerId);
    return outcome switch
    {
        "Success" => Results.Ok(new { message = "Шкафчик успешно назначен." }),
        "ClientNotFound" => Results.NotFound(new { error = "Клиент не найден." }),
        "LockerNotFound" => Results.NotFound(new { error = "Шкафчик не найден." }),
        "LockerOccupied" => Results.Conflict(new { error = "Этот шкафчик уже занят другим клиентом." }),
        "ClientAlreadyHasLocker" => Results.BadRequest(new { error = "У клиента уже есть назначенный шкафчик." }),
        _ => Results.BadRequest()
    };
});

clientsGroup.MapPost("/{clientId:guid}/additionalServices/{serviceId}", (Guid clientId, string serviceId, FitnessService service) =>
{
    var outcome = service.AddServiceToClient(clientId, serviceId);
    return outcome switch
    {
        "Success" => Results.Ok(new { message = "Услуга успешно подключена клиенту." }),
        "AlreadySubscribed" => Results.Ok(new { message = "Услуга уже была подключена ранее." }),
        "ClientNotFound" => Results.NotFound(new { error = "Клиент не найден." }),
        "ServiceNotFound" => Results.NotFound(new { error = "Услуга не найдена." }),
        _ => Results.BadRequest()
    };
});

#endregion

#region TRAINER ENDPOINTS

var trainersGroup = apiGroup.MapGroup("/trainers");

trainersGroup.MapPost("/", (Trainer trainer, FitnessService service) =>
{
    if (string.IsNullOrWhiteSpace(trainer.Surname) || string.IsNullOrWhiteSpace(trainer.Name))
        return Results.BadRequest(new { error = "Фамилия и Имя тренера обязательны." });

    var created = service.CreateTrainer(trainer);
    return Results.Created($"/api/trainers/{created.Id}", created);
});

trainersGroup.MapGet("/", (FitnessService service) => Results.Ok(service.GetAllTrainers()));

trainersGroup.MapPatch("/{id:guid}/status", (Guid id, [FromBody] TrainerStatus status, FitnessService service) =>
{
    var result = service.UpdateTrainerStatus(id, status);
    return result is not null ? Results.Ok(result) : Results.NotFound();
});

trainersGroup.MapGet("/{id:guid}/detail", (Guid id, FitnessService service) =>
{
    var detailDto = service.GetTrainerDetail(id);
    return detailDto is not null ? Results.Ok(detailDto) : Results.NotFound();
});

trainersGroup.MapPut("/{id:guid}", (Guid id, Trainer updatedTrainer, FitnessService service) =>
{
    var result = service.UpdateTrainer(id, updatedTrainer);
    return result is not null ? Results.Ok(result) : Results.NotFound();
});

#endregion

#region LOCKER ENDPOINTS (ТЗ 5.2)

var lockersGroup = apiGroup.MapGroup("/lockers");
lockersGroup.MapGet("/", (FitnessService service) => Results.Ok(service.GetAllLockers()));

#endregion

#region ADDITIONAL SERVICES ENDPOINTS (ТЗ 5.3)

var servicesGroup = apiGroup.MapGroup("/additionalServices");
servicesGroup.MapGet("/", (FitnessService service) => Results.Ok(service.GetAllServices()));
servicesGroup.MapGet("/{id}", (string id, FitnessService service) =>
{
    var detailDto = service.GetServiceDetail(id);
    return detailDto is not null ? Results.Ok(detailDto) : Results.NotFound();
});

#endregion

app.Run();
