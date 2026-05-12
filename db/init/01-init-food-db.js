const dbName = 'food_options_db';
const dbRef = db.getSiblingDB(dbName);

dbRef.createCollection('ingredients');
dbRef.createCollection('recipes');
dbRef.createCollection('mealPlanResults');

if (dbRef.ingredients.countDocuments() === 0) {
  dbRef.ingredients.insertMany([
    { name: 'Cucumber', availableQuantity: 2, unit: 'x' },
    { name: 'Olives', availableQuantity: 2, unit: 'x' },
    { name: 'Lettuce', availableQuantity: 3, unit: 'x' },
    { name: 'Meat', availableQuantity: 6, unit: 'x' },
    { name: 'Tomato', availableQuantity: 6, unit: 'x' },
    { name: 'Cheese', availableQuantity: 8, unit: 'x' },
    { name: 'Dough', availableQuantity: 10, unit: 'x' }
  ]);
}

if (dbRef.recipes.countDocuments() === 0) {
  dbRef.recipes.insertMany([
    {
      name: 'Burger',
      icon: 'lucideBeef',
      feeds: 1,
      requiredIngredients: [
        { ingredientName: 'Meat', quantity: 1, unit: 'x' },
        { ingredientName: 'Lettuce', quantity: 1, unit: 'x' },
        { ingredientName: 'Tomato', quantity: 1, unit: 'x' },
        { ingredientName: 'Cheese', quantity: 1, unit: 'x' },
        { ingredientName: 'Dough', quantity: 1, unit: 'x' }
      ]
    },
    {
      name: 'Pasta',
      icon: 'lucideCookingPot',
      feeds: 2,
      requiredIngredients: [
        { ingredientName: 'Dough', quantity: 2, unit: 'x' },
        { ingredientName: 'Tomato', quantity: 1, unit: 'x' },
        { ingredientName: 'Cheese', quantity: 2, unit: 'x' },
        { ingredientName: 'Meat', quantity: 1, unit: 'x' }
      ]
    },
    {
      name: 'Pie',
      icon: 'lucideCakeSlice',
      feeds: 1,
      requiredIngredients: [
        { ingredientName: 'Dough', quantity: 2, unit: 'x' },
        { ingredientName: 'Meat', quantity: 2, unit: 'x' }
      ]
    },
    {
      name: 'Salad',
      icon: 'lucideSalad',
      feeds: 3,
      requiredIngredients: [
        { ingredientName: 'Lettuce', quantity: 2, unit: 'x' },
        { ingredientName: 'Tomato', quantity: 2, unit: 'x' },
        { ingredientName: 'Cucumber', quantity: 1, unit: 'x' },
        { ingredientName: 'Cheese', quantity: 2, unit: 'x' },
        { ingredientName: 'Olives', quantity: 1, unit: 'x' }
      ]
    },
    {
      name: 'Sandwich',
      icon: 'lucideSandwich',
      feeds: 1,
      requiredIngredients: [
        { ingredientName: 'Dough', quantity: 1, unit: 'x' },
        { ingredientName: 'Cucumber', quantity: 1, unit: 'x' }
      ]
    },
    {
      name: 'Pizza',
      icon: 'lucidePizza',
      feeds: 4,
      requiredIngredients: [
        { ingredientName: 'Dough', quantity: 3, unit: 'x' },
        { ingredientName: 'Tomato', quantity: 2, unit: 'x' },
        { ingredientName: 'Cheese', quantity: 3, unit: 'x' },
        { ingredientName: 'Olives', quantity: 1, unit: 'x' }
      ]
    }
  ]);
}
