using backend.Application.Interfaces;
using backend.Models;
using MongoDB.Driver;

namespace backend.Infrastructure.Repositories;

public class RecipeRepository : IRecipeRepository
{
    private readonly IMongoCollection<Recipe> _collection;

    public RecipeRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Recipe>("recipes");
    }

    public async Task<List<Recipe>> GetAllAsync(CancellationToken ct = default)
    {
        return await _collection.Find(_ => true).ToListAsync(ct);
    }

    public async Task<List<Recipe>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken ct = default)
    {
        var idList = ids.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
        return await _collection.Find(x => idList.Contains(x.Id)).ToListAsync(ct);
    }
}
