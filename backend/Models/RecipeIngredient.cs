using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models;

public class RecipeIngredient
{
    [BsonElement("ingredientName")]
    public string IngredientName { get; set; } = string.Empty;

    [BsonElement("quantity")]
    public int Quantity { get; set; }

    [BsonElement("unit")]
    public string Unit { get; set; } = "x";
}
