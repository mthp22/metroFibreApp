using backend.Models;

namespace backend.Application.Interfaces;

public interface IRecipeRepository
{
    Task<List<Recipe>> GetAllAsync(CancellationToken ct = default);
    Task<List<Recipe>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken ct = default);
}
