using CharacterLib;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Bind to PORT environment variable (required for Railway/Render/Heroku)
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Character API", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// -------------------------------------------------------
// GET /health  -  health check (обов'язковий endpoint)
// -------------------------------------------------------
app.MapGet("/health", () => Results.Ok(new
{
    status = "ok",
    service = "Character API",
    version = "1.0.0",
    timestamp = DateTime.UtcNow
}));

// -------------------------------------------------------
// POST /character  -  створити персонажа
// Body: { "level":1, "health":100, "energy":50, "experience":0 }
// -------------------------------------------------------
app.MapPost("/character", ([FromBody] CreateCharacterRequest req) =>
{
    try
    {
        var ch = new Character(req.Level, req.Health, req.Energy, req.Experience);
        return Results.Ok(CharacterToDto(ch));
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

// -------------------------------------------------------
// POST /character/take-damage
// Body: { "level":1, "health":100, "energy":50, "experience":0, "damage":30 }
// -------------------------------------------------------
app.MapPost("/character/take-damage", ([FromBody] DamageRequest req) =>
{
    try
    {
        var ch = new Character(req.Level, req.Health, req.Energy, req.Experience);
        ch.TakeDamage(req.Damage);
        return Results.Ok(CharacterToDto(ch));
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(new { error = ex.Message });
    }
});

// -------------------------------------------------------
// POST /character/upgrade
// Body: { "level":1, "health":100, "energy":50, "experience":100 }
// -------------------------------------------------------
app.MapPost("/character/upgrade", ([FromBody] CreateCharacterRequest req) =>
{
    try
    {
        var ch = new Character(req.Level, req.Health, req.Energy, req.Experience);
        bool upgraded = ch.Upgrade();
        return Results.Ok(new { upgraded, character = CharacterToDto(ch) });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

// -------------------------------------------------------
// POST /character/use-energy
// Body: { "level":1, "health":100, "energy":50, "experience":0, "amount":20 }
// -------------------------------------------------------
app.MapPost("/character/use-energy", ([FromBody] EnergyRequest req) =>
{
    try
    {
        var ch = new Character(req.Level, req.Health, req.Energy, req.Experience);
        ch.UseEnergy(req.Amount);
        return Results.Ok(CharacterToDto(ch));
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(new { error = ex.Message });
    }
});

// -------------------------------------------------------
// POST /character/gain-experience
// Body: { "level":1, "health":100, "energy":50, "experience":0, "amount":50 }
// -------------------------------------------------------
app.MapPost("/character/gain-experience", ([FromBody] ExperienceRequest req) =>
{
    try
    {
        var ch = new Character(req.Level, req.Health, req.Energy, req.Experience);
        ch.GainExperience(req.Amount);
        return Results.Ok(CharacterToDto(ch));
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

// -------------------------------------------------------
// POST /character/profile
// Body: { "level":1, "health":100, "energy":50, "experience":0 }
// -------------------------------------------------------
app.MapPost("/character/profile", ([FromBody] CreateCharacterRequest req) =>
{
    try
    {
        var ch = new Character(req.Level, req.Health, req.Energy, req.Experience);
        return Results.Ok(new { profile = ch.GenerateProfile() });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.Run();

// -------------------------------------------------------
// Helpers
// -------------------------------------------------------
static object CharacterToDto(Character ch) => new
{
    level = ch.Level,
    health = ch.Health,
    energy = ch.Energy,
    experience = ch.Experience,
    isAlive = ch.IsAlive,
    maxHealth = Character.MaxHealth,
    maxEnergy = Character.MaxEnergy,
    experiencePerLevel = Character.ExperiencePerLevel,
    profile = ch.GenerateProfile()
};

// -------------------------------------------------------
// Request DTOs
// -------------------------------------------------------
record CreateCharacterRequest(int Level, int Health, int Energy, int Experience);
record DamageRequest(int Level, int Health, int Energy, int Experience, int Damage);
record EnergyRequest(int Level, int Health, int Energy, int Experience, int Amount);
record ExperienceRequest(int Level, int Health, int Energy, int Experience, int Amount);
