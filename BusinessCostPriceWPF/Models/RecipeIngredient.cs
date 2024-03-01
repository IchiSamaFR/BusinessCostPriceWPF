using BusinessCostPriceWPF.Services;
using BusinessCostPriceWPF.Services.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCostPriceWPF.Models
{
    public partial class RecipeIngredient : ObservableObject
    {
        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        private double _quantity;
        [ObservableProperty]
        public IIngredient _ingredient;
        [ObservableProperty]
        public double _price;

        public RecipeIngredient Fill(RecipeIngredientDTO recipeIngredient)
        {
            Id = recipeIngredient.Id;
            Quantity = recipeIngredient.Quantity;
            Ingredient = recipeIngredient.IIngredient is IngredientDTO ? Models.Ingredient.Build(recipeIngredient.IIngredient as IngredientDTO) : Recipe.Build(recipeIngredient.IIngredient as RecipeDTO);
            Price = recipeIngredient.Price;
            return this;
        }
        public static RecipeIngredient Build(RecipeIngredientDTO recipeIngredient)
        {
            return new RecipeIngredient().Fill(recipeIngredient);
        }
        public static RecipeIngredientDTO BuildDTO(RecipeIngredient recipeIngredient)
        {
            return new RecipeIngredientDTO()
            {
                Id = recipeIngredient.Id,
                Quantity = recipeIngredient.Quantity,
                Price = recipeIngredient.Price
            };
        }
    }
}
