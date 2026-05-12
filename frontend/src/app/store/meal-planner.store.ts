import { computed, inject, Injectable, signal } from '@angular/core';
import { forkJoin, finalize } from 'rxjs';
import { Ingredient } from '../models/ingredient.model';
import { MealPlanResult } from '../models/meal-plan.model';
import { Recipe } from '../models/recipe.model';
import { MealPlannerService } from '../services/meal-planner.service';

@Injectable({ providedIn: 'root' })
export class MealPlannerStore {
  private readonly service = inject(MealPlannerService);

  readonly recipes = signal<Recipe[]>([]);
  readonly ingredients = signal<Ingredient[]>([]);
  readonly selectedRecipeIds = signal<string[]>([]);
  readonly selectedQuantities = signal<Record<string, number>>({});
  readonly availabilityByRecipeId = signal<Record<string, boolean>>({});
  readonly result = signal<MealPlanResult | null>(null);
  readonly loading = signal(false);

  readonly totalPeopleFed = computed(() => this.result()?.totalPeopleFed ?? 0);

  load() {
    this.loading.set(true);
    forkJoin({ recipes: this.service.getRecipes(), ingredients: this.service.getIngredients() })
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe(({ recipes, ingredients }) => {
        this.recipes.set(recipes);
        this.ingredients.set(ingredients);
        this.refreshRealtime();
      });
  }

  toggleRecipe(id: string) {
    const ids = this.selectedRecipeIds();
    const next = ids.includes(id) ? ids.filter((x) => x !== id) : [...ids, id];
    this.selectedRecipeIds.set(next);

    const qty = { ...this.selectedQuantities() };
    if (!next.includes(id)) {
      delete qty[id];
    } else if (!qty[id] || qty[id] < 1) {
      qty[id] = 1;
    }
    this.selectedQuantities.set(qty);
    this.refreshRealtime();
  }

  setQuantity(id: string, quantity: number) {
    const normalized = Number.isFinite(quantity) ? Math.max(0, Math.floor(quantity)) : 0;
    this.selectedQuantities.set({ ...this.selectedQuantities(), [id]: normalized });
    this.refreshRealtime();
  }

  calculate() {
    this.loading.set(true);
    const request = this.currentRequest();
    this.service
      .calculate(request)
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe((result) => {
        this.result.set(result);
        this.refreshAvailabilityOnly();
      });
  }

  advise() {
    this.loading.set(true);
    this.service
      .advise()
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe((result) => this.result.set(result));
  }

  canPrepareRecipe(id: string) {
    return this.availabilityByRecipeId()[id] ?? true;
  }

  private refreshRealtime() {
    const request = this.currentRequest();
    this.service.preview(request).subscribe({
      next: (result) => {
        this.result.set(result);
        this.refreshAvailabilityOnly();
      },
      error: () => {
        this.refreshAvailabilityOnly();
      }
    });
  }

  private refreshAvailabilityOnly() {
    this.service.availability(this.currentRequest()).subscribe((availability) => {
      this.availabilityByRecipeId.set(availability);
    });
  }

  private currentRequest() {
    const selectedIds = this.selectedRecipeIds();
    const quantities = selectedIds.reduce<Record<string, number>>((acc, id) => {
      const q = this.selectedQuantities()[id];
      acc[id] = Number.isFinite(q) && q >= 0 ? Math.floor(q) : 1;
      return acc;
    }, {});

    return { selectedRecipeIds: selectedIds, quantities };
  }
}
