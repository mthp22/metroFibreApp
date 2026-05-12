import { RemainingIngredient } from './ingredient.model';
import { RecipeIngredient } from './recipe.model';

export interface SelectedMealResult {
  mealName: string;
  icon: string;
  quantityPrepared: number;
  peopleFed: number;
  requiredIngredients: RecipeIngredient[];
}

export interface MealPlanResult {
  totalPeopleFed: number;
  selectedMeals: SelectedMealResult[];
  remainingIngredients: RemainingIngredient[];
  isAdvisorResult: boolean;
  createdAtUtc: string;
}

export interface CalculateMealPlanRequest {
  selectedRecipeIds: string[];
  quantities: Record<string, number>;
}
