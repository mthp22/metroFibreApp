import { Component, inject } from '@angular/core';
import { RecipeSelector } from '../../components/recipe-selector/recipe-selector';
import { IngredientUsage } from '../../components/ingredient-usage/ingredient-usage';
import { RemainingSummary } from '../../components/remaining-summary/remaining-summary';
import { MealPlannerStore } from '../../store/meal-planner.store';

@Component({
  selector: 'app-meal-planner',
  standalone: true,
  imports: [RecipeSelector, IngredientUsage, RemainingSummary],
  templateUrl: './meal-planner.html',
  styleUrl: './meal-planner.scss'
})
export class MealPlanner {
  readonly store = inject(MealPlannerStore);

  constructor() {
    this.store.load();
  }
}
