namespace backend.DTOs;

public class CalculateMealPlanRequest
{
    public List<string> SelectedRecipeIds { get; set; } = [];
    public Dictionary<string, int> Quantities { get; set; } = new();
}
