using FitnessPP.Dtos;
using FitnessPP.Models;
using FitnessPP.Repositories;
using FitnessPP.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// 1. –≈√»—“–ј÷»я —≈–¬»—ќ¬ (—лой јрхитектуры)

// –егистрируем репозитории как Singleton, чтобы данные не стирались между HTTP-запросами
builder.Services.AddSingleton<IClientRepository, InMemoryClientRepository>();
builder.Services.AddSingleton<ITrainerRepository, InMemoryTrainerRepository>();

// –егистрируем наш сервис бизнес-логики
builder.Services.AddSingleton<FitnessService>();

// ¬ключаем поддержку OpenAPI/Swagger дл€ автоматической документации API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ¬ключаем Swagger-интерфейс в режиме разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 2. –≈јЋ»«ј÷»я ћј–Ў–”“ќ¬ API (Ёндпоинты из “«)

// √руппируем все маршруты под базовый путь /api
var apiGroup = app.MapGroup("/api");

#region CLIENT ENDPOINTS (/api/clients)

var clientsGroup = apiGroup.MapGroup("/clients");

// POST /api/clients Ч —оздать клиента (с ручной валидацией по “«)
clientsGroup.MapPost("/", (Client client, FitnessService service) =>
{
    if (string.IsNullOrWhiteSpace(client.Surname) || string.IsNullOrWhiteSpace(client.Name))
    {
        return Results.BadRequest(new { error = "‘амили€ и »м€ €вл€ютс€ об€зательными пол€ми." });
    }
    if (string.IsNullOrWhiteSpace(client.Email) || !client.Email.Contains('@'))
    {
        return Results.BadRequest(new { error = "Ќекорректный формат Email." });
    }

    var created = service.CreateClient(client);
    return Results.Created($"/api/clients/{created.Id}", created);
});

// GET /api/clients Ч ѕолучить список всех клиентов
clientsGroup.MapGet("/", (FitnessService service) =>
    Results.Ok(service.GetAllClients()));

// GET /api/clients/{id} Ч  ратка€ информаци€ о клиенте
clientsGroup.MapGet("/{id:guid}", (Guid id, FitnessService service) =>
{
    var client = service.GetClientById(id);
    return client is not null ? Results.Ok(client) : Results.NotFound();
});

// GET /api/clients/{id}/detail Ч ѕодробна€ информаци€ с вложенным тренером (ѕункт 4.4)
clientsGroup.MapGet("/{id:guid}/detail", (Guid id, FitnessService service) =>
{
    var detailDto = service.GetClientDetail(id);
    return detailDto is not null ? Results.Ok(detailDto) : Results.NotFound();
});

// PUT /api/clients/{id} Ч ќбновить данные клиента
clientsGroup.MapPut("/{id:guid}", (Guid id, Client updatedClient, FitnessService service) =>
{
    var result = service.UpdateClient(id, updatedClient);
    return result is not null ? Results.Ok(result) : Results.NotFound();
});

// PATCH /api/clients/{id}/status Ч »зменить статус активности клиента
clientsGroup.MapPatch("/{id:guid}/status", (Guid id, [FromBody] bool isActive, FitnessService service) =>
{
    var result = service.UpdateClientStatus(id, isActive);
    return result is not null ? Results.Ok(result) : Results.NotFound();
});

// POST /api/clients/{clientId}/trainer/{trainerId} Ч Ќазначить тренера клиенту
clientsGroup.MapPost("/{clientId:guid}/trainer/{trainerId:guid}", (Guid clientId, Guid trainerId, FitnessService service) =>
{
    var outcome = service.AssignTrainer(clientId, trainerId);
    return outcome switch
    {
        "Success" => Results.Ok(new { message = "“ренер успешно назначен клиенту." }),
        "ClientNotFound" => Results.NotFound(new { error = $" лиент с ID {clientId} не найден." }),
        "TrainerNotFound" => Results.BadRequest(new { error = $"“ренер с ID {trainerId} не найден." }),
        _ => Results.BadRequest()
    };
});

#endregion

#region TRAINER ENDPOINTS (/api/trainers)

var trainersGroup = apiGroup.MapGroup("/trainers");

// POST /api/trainers Ч —оздать тренера
trainersGroup.MapPost("/", (Trainer trainer, FitnessService service) =>
{
    if (string.IsNullOrWhiteSpace(trainer.Surname) || string.IsNullOrWhiteSpace(trainer.Name))
    {
        return Results.BadRequest(new { error = "‘амили€ и »м€ тренера об€зательны." });
    }

    var created = service.CreateTrainer(trainer);
    return Results.Created($"/api/trainers/{created.Id}", created);
});

// GET /api/trainers Ч ѕолучить список всех тренеров
trainersGroup.MapGet("/", (FitnessService service) =>
    Results.Ok(service.GetAllTrainers()));

// PATCH /api/trainers/{id}/status Ч »зменить статус тренера (WORKING, ON_LEAVE, NOT_WORKING)
trainersGroup.MapPatch("/{id:guid}/status", (Guid id, [FromBody] TrainerStatus status, FitnessService service) =>
{
    var result = service.UpdateTrainerStatus(id, status);
    return result is not null ? Results.Ok(result) : Results.NotFound();
});

// GET /api/trainers/{id}/detail Ч ѕодробна€ информаци€ с массивом его клиентов
trainersGroup.MapGet("/{id:guid}/detail", (Guid id, FitnessService service) =>
{
    var detailDto = service.GetTrainerDetail(id);
    return detailDto is not null ? Results.Ok(detailDto) : Results.NotFound();
});

// PUT /api/trainers/{id} Ч ќбновить данные тренера
trainersGroup.MapPut("/{id:guid}", (Guid id, Trainer updatedTrainer, FitnessService service) =>
{
    var result = service.UpdateTrainer(id, updatedTrainer);
    return result is not null ? Results.Ok(result) : Results.NotFound();
});

#endregion

app.Run();
