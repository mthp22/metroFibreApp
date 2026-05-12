namespace backend.Models;

public class SelectedMealResult
{
    public string MealName { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public int QuantityPrepared { get; set; }
    public int PeopleFed { get; set; }
    public List<RecipeIngredient> RequiredIngredients { get; set; } = [];
}
