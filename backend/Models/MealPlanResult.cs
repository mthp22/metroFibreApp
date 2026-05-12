using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models;

public class MealPlanResult
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public int TotalPeopleFed { get; set; }
    public List<SelectedMealResult> SelectedMeals { get; set; } = [];
    public List<RemainingIngredient> RemainingIngredients { get; set; } = [];
    public bool IsAdvisorResult { get; set; }
}
