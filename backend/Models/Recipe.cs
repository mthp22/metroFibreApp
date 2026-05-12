using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models;

public class Recipe
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("icon")]
    public string Icon { get; set; } = string.Empty;

    [BsonElement("feeds")]
    public int Feeds { get; set; }

    [BsonElement("requiredIngredients")]
    public List<RecipeIngredient> RequiredIngredients { get; set; } = [];
}
