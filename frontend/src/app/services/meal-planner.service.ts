import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Ingredient } from '../models/ingredient.model';
import { CalculateMealPlanRequest, MealPlanResult } from '../models/meal-plan.model';
import { Recipe } from '../models/recipe.model';

@Injectable({ providedIn: 'root' })
export class MealPlannerService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = 'http://localhost:5098/api';

  getIngredients() {
    return this.http.get<Ingredient[]>(`${this.baseUrl}/ingredients`);
  }

  getRecipes() {
    return this.http.get<Recipe[]>(`${this.baseUrl}/recipes`);
  }

  calculate(request: CalculateMealPlanRequest) {
    return this.http.post<MealPlanResult>(`${this.baseUrl}/mealplan/calculate`, request);
  }

  preview(request: CalculateMealPlanRequest) {
    return this.http.post<MealPlanResult>(`${this.baseUrl}/mealplan/preview`, request);
  }

  availability(request: CalculateMealPlanRequest) {
    return this.http.post<Record<string, boolean>>(`${this.baseUrl}/mealplan/availability`, request);
  }

  advise() {
    return this.http.post<MealPlanResult>(`${this.baseUrl}/mealplan/advise`, {});
  }
}
