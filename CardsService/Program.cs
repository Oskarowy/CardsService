using CardsService.Policies;
using CardsService.Services;

var builder = WebApplication.CreateBuilder(args);
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

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
