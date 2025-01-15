using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddSingleton<IExternalUserCardService, ExternalUserCardServiceMock>();
builder.Services.AddSingleton<ActionService>();
builder.Services.AddSingleton<IActionPolicy, Action1Policy>();
builder.Services.AddSingleton<IActionPolicy, Action2Policy>();
builder.Services.AddSingleton<IActionPolicy, Action3Policy>();
builder.Services.AddSingleton<IActionPolicy, Action4Policy>();
builder.Services.AddSingleton<IActionPolicy, Action5Policy>();
builder.Services.AddSingleton<IActionPolicy, Action6Policy>();
builder.Services.AddSingleton<IActionPolicy, Action7Policy>();
builder.Services.AddSingleton<IActionPolicy, Action8Policy>();
builder.Services.AddSingleton<IActionPolicy, Action9Policy>();
builder.Services.AddSingleton<IActionPolicy, Action10Policy>();
builder.Services.AddSingleton<IActionPolicy, Action11Policy>();
builder.Services.AddSingleton<IActionPolicy, Action12Policy>();
builder.Services.AddSingleton<IActionPolicy, Action13Policy>();
builder.Services.AddScoped<CardService>();

builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "CardService API is running!");

app.MapPost("/api/allowed-actions", async (CardRequest? request, CardService cardService) =>
{
    if (request == null)
    {
        return Results.BadRequest(new { error = "Request body is missing or malformed." });
    }

    if (string.IsNullOrWhiteSpace(request.UserId))
    {
        return Results.BadRequest(new { error = "Parameter 'userId' is missing, empty or has incorrect type." });
    }

    if (string.IsNullOrWhiteSpace(request.CardNumber))
    {
        return Results.BadRequest(new { error = "Parameter 'cardNumber' is missing, empty or has incorrect type." });
    }

    try
    {
        var allowedActions = await cardService.GetCardAllowedActions(request.UserId, request.CardNumber);
        return Results.Ok(new { allowedActions });
    }
    catch (KeyNotFoundException)
    {
        return Results.BadRequest(new { error = "Cannot find any users with UserId : " + request.UserId });
    }
    catch (ArgumentException)
    {
        return Results.BadRequest(new { error = "Cannot find any cards with card number " + request.CardNumber + " for UserId " + request.UserId });
    }
})
.WithName("GetAllowedActions")
.Produces(200, typeof(object))
.Produces(400, typeof(object));

app.MapHealthChecks("/health");

app.Run();
