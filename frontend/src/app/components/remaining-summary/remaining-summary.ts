import { Component, input } from '@angular/core';
import { Ingredient, RemainingIngredient } from '../../models/ingredient.model';

@Component({
  selector: 'app-remaining-summary',
  standalone: true,
  imports: [],
  templateUrl: './remaining-summary.html',
  styleUrl: './remaining-summary.scss'
})
export class RemainingSummary {
  readonly ingredients = input<Ingredient[]>([]);
  readonly remaining = input<RemainingIngredient[]>([]);
  readonly totalPeopleFed = input(0);
}
