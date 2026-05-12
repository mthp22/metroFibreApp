using backend.Models;

namespace backend.Application.Interfaces;

public interface IMealPlanResultRepository
{
    Task SaveAsync(MealPlanResult result, CancellationToken ct = default);
}
