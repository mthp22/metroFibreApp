using backend.Application.Interfaces;
using backend.DTOs;
using backend.Models;

namespace backend.Application.Services;

public class MealPlannerService : IMealPlannerService
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IMealPlanResultRepository _resultRepository;

    public MealPlannerService(
        IIngredientRepository ingredientRepository,
        IRecipeRepository recipeRepository,
        IMealPlanResultRepository resultRepository)
    {
        _ingredientRepository = ingredientRepository;
        _recipeRepository = recipeRepository;
        _resultRepository = resultRepository;
    }

    public async Task<MealPlanResult> CalculateAsync(CalculateMealPlanRequest request, CancellationToken ct = default)
    {
        var result = await PreviewAsync(request, ct);
        await _resultRepository.SaveAsync(result, ct);
        return result;
    }

    public async Task<MealPlanResult> PreviewAsync(CalculateMealPlanRequest request, CancellationToken ct = default)
    {
        var ingredients = await _ingredientRepository.GetAllAsync(ct);
        var recipes = await _recipeRepository.GetByIdsAsync(request.SelectedRecipeIds, ct);
        return BuildFromQuantities(ingredients, recipes, request);
    }

    public async Task<Dictionary<string, bool>> AvailabilityAsync(CalculateMealPlanRequest request, CancellationToken ct = default)
    {
        var ingredients = await _ingredientRepository.GetAllAsync(ct);
        var allRecipes = await _recipeRepository.GetAllAsync(ct);
        var selectedRecipes = allRecipes.Where(r => request.SelectedRecipeIds.Contains(r.Id)).ToList();

        var current = BuildFromQuantities(ingredients, selectedRecipes, request);
        var remaining = current.RemainingIngredients.ToDictionary(x => x.Name, x => x.RemainingQuantity);

        return allRecipes.ToDictionary(
            recipe => recipe.Id,
            recipe => recipe.RequiredIngredients.All(req => remaining.GetValueOrDefault(req.IngredientName) >= req.Quantity));
    }

    public async Task<MealPlanResult> AdviseAsync(CancellationToken ct = default)
    {
        var ingredients = await _ingredientRepository.GetAllAsync(ct);
        var recipes = await _recipeRepository.GetAllAsync(ct);
        var result = Optimize(ingredients, recipes, true);
        await _resultRepository.SaveAsync(result, ct);
        return result;
    }

    private static MealPlanResult BuildFromQuantities(List<Ingredient> ingredients, List<Recipe> recipes, CalculateMealPlanRequest request)
    {
        var requestedIds = request.SelectedRecipeIds.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToHashSet();
        var counts = recipes.Select(r => requestedIds.Contains(r.Id) ? Math.Max(0, request.Quantities.GetValueOrDefault(r.Id, 1)) : 0).ToArray();

        var stock = ingredients.ToDictionary(i => i.Name, i => i.AvailableQuantity);
        if (!CanBuild(counts, recipes, stock, out _))
        {
            throw new InvalidOperationException("Insufficient ingredients for selected meal quantities.");
        }

        return BuildResultFromCounts(ingredients, recipes, counts, false);
    }

    private static MealPlanResult Optimize(List<Ingredient> availableIngredients, List<Recipe> recipes, bool isAdvisorResult)
    {
        var ingredientStock = availableIngredients.ToDictionary(i => i.Name, i => i.AvailableQuantity);
        var bestCounts = new int[recipes.Count];
        var currentCounts = new int[recipes.Count];
        var bestTotalFed = 0;
        var bestRemaining = int.MaxValue;

        void Search(int index)
        {
            if (index == recipes.Count)
            {
                if (!CanBuild(currentCounts, recipes, ingredientStock, out var remainingStock)) return;

                var totalFed = recipes.Select((recipe, i) => recipe.Feeds * currentCounts[i]).Sum();
                var totalRemaining = remainingStock.Values.Sum();

                if (totalFed > bestTotalFed || (totalFed == bestTotalFed && totalRemaining < bestRemaining))
                {
                    bestTotalFed = totalFed;
                    bestRemaining = totalRemaining;
                    Array.Copy(currentCounts, bestCounts, currentCounts.Length);
                }
                return;
            }

            var maxPossible = MaxRecipeCount(recipes[index], ingredientStock);
            for (var quantity = 0; quantity <= maxPossible; quantity++)
            {
                currentCounts[index] = quantity;
                Search(index + 1);
            }
            currentCounts[index] = 0;
        }

        Search(0);
        return BuildResultFromCounts(availableIngredients, recipes, bestCounts, isAdvisorResult);
    }

    private static MealPlanResult BuildResultFromCounts(List<Ingredient> availableIngredients, List<Recipe> recipes, int[] counts, bool isAdvisorResult)
    {
        var ingredientStock = availableIngredients.ToDictionary(i => i.Name, i => i.AvailableQuantity);
        _ = CanBuild(counts, recipes, ingredientStock, out var finalRemaining);

        var selectedMeals = recipes
            .Select((recipe, i) => new { recipe, quantity = counts[i] })
            .Where(x => x.quantity > 0)
            .Select(x => new SelectedMealResult
            {
                MealName = x.recipe.Name,
                Icon = x.recipe.Icon,
                QuantityPrepared = x.quantity,
                PeopleFed = x.quantity * x.recipe.Feeds,
                RequiredIngredients = x.recipe.RequiredIngredients
            })
            .OrderByDescending(x => x.PeopleFed)
            .ToList();

        return new MealPlanResult
        {
            IsAdvisorResult = isAdvisorResult,
            TotalPeopleFed = selectedMeals.Sum(x => x.PeopleFed),
            SelectedMeals = selectedMeals,
            RemainingIngredients = availableIngredients.Select(i => new RemainingIngredient
            {
                Name = i.Name,
                Unit = i.Unit,
                RemainingQuantity = finalRemaining.GetValueOrDefault(i.Name)
            }).ToList()
        };
    }

    private static bool CanBuild(int[] recipeCounts, List<Recipe> recipes, Dictionary<string, int> ingredientStock, out Dictionary<string, int> remainingStock)
    {
        remainingStock = ingredientStock.ToDictionary(x => x.Key, x => x.Value);
        for (var i = 0; i < recipes.Count; i++)
        {
            var count = recipeCounts[i];
            if (count == 0) continue;
            foreach (var req in recipes[i].RequiredIngredients)
            {
                var needed = req.Quantity * count;
                var has = remainingStock.GetValueOrDefault(req.IngredientName, 0);
                if (has < needed) return false;
                remainingStock[req.IngredientName] = has - needed;
            }
        }
        return true;
    }

    private static int MaxRecipeCount(Recipe recipe, Dictionary<string, int> ingredientStock)
    {
        if (recipe.RequiredIngredients.Count == 0) return 0;
        return recipe.RequiredIngredients
            .Select(req => req.Quantity == 0 ? 0 : ingredientStock.GetValueOrDefault(req.IngredientName, 0) / req.Quantity)
            .DefaultIfEmpty(0)
            .Min();
    }
}
