using backend.Models;

namespace backend.Application.Interfaces;

public interface IIngredientRepository
{
    Task<List<Ingredient>> GetAllAsync(CancellationToken ct = default);
}
