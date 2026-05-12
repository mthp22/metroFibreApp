using backend.Application.Interfaces;
using backend.Models;
using MongoDB.Driver;

namespace backend.Infrastructure.Repositories;

public class IngredientRepository : IIngredientRepository
{
    private readonly IMongoCollection<Ingredient> _collection;

    public IngredientRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Ingredient>("ingredients");
    }

    public async Task<List<Ingredient>> GetAllAsync(CancellationToken ct = default)
    {
        return await _collection.Find(_ => true).ToListAsync(ct);
    }
}
