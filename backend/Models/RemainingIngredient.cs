namespace backend.Models;

public class RemainingIngredient
{
    public string Name { get; set; } = string.Empty;
    public int RemainingQuantity { get; set; }
    public string Unit { get; set; } = "x";
}
