using DnDEncounterGenerator.Configuration;
using DnDEncounterGenerator.Data;
using DnDEncounterGenerator.Data.Models.Entity;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration.GetSection("EncounterGenerator")
    .Get<EncounterGeneratorConfiguration>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IEncounterGeneratorConfiguration>(config);
builder.Services.AddScoped<IMongoClient, MongoClient>(_ => new MongoClient(config.ConnectionString));
builder.Services.AddScoped<IRepository<Monster>, MongoDbRepository<Monster>>();

if (!BsonClassMap.IsClassMapRegistered(typeof(Monster)))
{
    BsonClassMap.RegisterClassMap<Monster>(cm =>
    {
        cm.MapIdMember(m => m.Id).SetOrder(0);
    });
}

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/monster", async (Monster monster, IRepository<Monster> repository) =>
    await repository.AddEntity(monster)
    ? Results.CreatedAtRoute("GetMonster", new { id = monster.Id}, monster)
    : Results.BadRequest());

app.MapPost("/monsters", async (Monster[] monsters, IRepository<Monster> repository) =>
    await repository.AddEntities(monsters)
        ? Results.CreatedAtRoute("GetMonsters", value: monsters)
        : Results.BadRequest());

app.MapGet("/monster/{id}", async (Guid id, IRepository<Monster> repository) => 
    await repository.GetEntity(id)
        is Monster monster
        ? Results.Ok(monster)
        : Results.NotFound())
    .WithName("GetMonster");

app.MapGet("/monsters", async (IRepository<Monster> repository) =>
    await repository.ListEntities()
        is IEnumerable<Monster> monster
        ? Results.Ok(monster)
        : Results.NotFound())
    .WithName("GetMonsters");

app.MapDelete("/monster/{id}", async (Guid id, IRepository<Monster> repository) =>
    await repository.DeleteEntity(id)
        ? Results.NoContent()
        : Results.NotFound());

app.Run();