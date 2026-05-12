using backend.Application.Interfaces;
using backend.Models;
using MongoDB.Driver;

namespace backend.Infrastructure.Repositories;

public class MealPlanResultRepository : IMealPlanResultRepository
{
    private readonly IMongoCollection<MealPlanResult> _collection;

    public MealPlanResultRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<MealPlanResult>("mealPlanResults");
    }

    public async Task SaveAsync(MealPlanResult result, CancellationToken ct = default)
    {
        await _collection.InsertOneAsync(result, cancellationToken: ct);
    }
}
