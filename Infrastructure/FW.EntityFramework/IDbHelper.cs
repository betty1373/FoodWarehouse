using FW.Domain;

namespace FW.EntityFramework
{
    public interface IDbHelper
    {
        public void SetProducts(Products products);

        public void SetRecipes(Recipes recipe);

        public void SetIngredients(Ingredients ingredients);
    }
}
