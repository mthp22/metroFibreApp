import { Component, input } from '@angular/core';
import { SelectedMealResult } from '../../models/meal-plan.model';

@Component({
  selector: 'app-ingredient-usage',
  standalone: true,
  imports: [],
  templateUrl: './ingredient-usage.html',
  styleUrl: './ingredient-usage.scss'
})
export class IngredientUsage {
  readonly meals = input<SelectedMealResult[]>([]);
}
