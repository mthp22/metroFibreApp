export interface RecipeIngredient {
  ingredientName: string;
  quantity: number;
  unit: string;
}

export interface Recipe {
  id: string;
  name: string;
  icon: string;
  feeds: number;
  requiredIngredients: RecipeIngredient[];
}
