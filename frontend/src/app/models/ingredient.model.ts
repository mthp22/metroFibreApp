export interface Ingredient {
  id: string;
  name: string;
  availableQuantity: number;
  unit: string;
}

export interface RemainingIngredient {
  name: string;
  remainingQuantity: number;
  unit: string;
}
