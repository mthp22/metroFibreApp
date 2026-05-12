using MongoDB.Bson;
using MongoDB.Driver;

namespace backend.Infrastructure.Initialization;

public class MongoDbInitializer
{
    private readonly IMongoDatabase _database;

    public MongoDbInitializer(IMongoDatabase database)
    {
        _database = database;
    }

    public async Task InitializeAsync(CancellationToken ct = default)
    {
        var existing = await _database.ListCollectionNames().ToListAsync(ct);
        var required = new[] { "ingredients", "recipes", "mealPlanResults" };

        foreach (var name in required)
        {
            if (!existing.Contains(name))
            {
                await _database.CreateCollectionAsync(name, cancellationToken: ct);
            }
        }

        var ingredients = _database.GetCollection<BsonDocument>("ingredients");
        var recipes = _database.GetCollection<BsonDocument>("recipes");

        if (await ingredients.CountDocumentsAsync(FilterDefinition<BsonDocument>.Empty, cancellationToken: ct) == 0 ||
            await recipes.CountDocumentsAsync(FilterDefinition<BsonDocument>.Empty, cancellationToken: ct) == 0)
        {
            throw new InvalidOperationException("MongoDB seed data missing. Ensure docker init scripts are mounted.");
        }
    }
}
