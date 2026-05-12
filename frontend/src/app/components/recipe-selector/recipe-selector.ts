import { Component, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Recipe } from '../../models/recipe.model';

@Component({
  selector: 'app-recipe-selector',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './recipe-selector.html',
  styleUrl: './recipe-selector.scss',
})
export class RecipeSelector {
  readonly recipe = input.required<Recipe>();
  readonly selected = input(false);
  readonly quantity = input(0);
  readonly canPrepare = input(true);
  readonly toggled = output<string>();
  readonly quantityChanged = output<{ id: string; quantity: number }>();

  onQuantityChanged(value: string) {
    const parsed = Number.parseInt(value, 10);
    const quantity = Number.isFinite(parsed) ? Math.max(0, parsed) : 0;
    this.quantityChanged.emit({ id: this.recipe().id, quantity });
  }
}
