using backend.DTOs;
using backend.Models;

namespace backend.Application.Interfaces;

public interface IMealPlannerService
{
    Task<MealPlanResult> CalculateAsync(CalculateMealPlanRequest request, CancellationToken ct = default);
    Task<MealPlanResult> PreviewAsync(CalculateMealPlanRequest request, CancellationToken ct = default);
    Task<Dictionary<string, bool>> AvailabilityAsync(CalculateMealPlanRequest request, CancellationToken ct = default);
    Task<MealPlanResult> AdviseAsync(CancellationToken ct = default);
}
