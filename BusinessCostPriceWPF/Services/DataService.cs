using BusinessCostPriceWPF.Models;
using BusinessCostPriceWPF.Views.Pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCostPriceWPF.Services
{
    public static class DataService
    {
        private static string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BusinessCostPriceWPF");
        public static IIngredient DeletedIngredient { get; } = new Ingredient("0", "Supprimé", Resources.Enums.Unit.kilogram, 0);

        public static ObservableCollection<Ingredient> Ingredients = new ObservableCollection<Ingredient>();
        public static ObservableCollection<Recipe> Recipes = new ObservableCollection<Recipe>();
        public static ObservableCollection<Furniture> Furnitures = new ObservableCollection<Furniture>();
        public static IEnumerable<Ingredient> GetLastIngredients
        {
            get
            {
                return Ingredients.GroupBy(p => p.Id).Select(g => g.OrderByDescending(p => p.Date).FirstOrDefault());
            }
        }
        public static IEnumerable<Recipe> GetLastRecipes
        {
            get
            {
                return Recipes.GroupBy(p => p.Id).Select(g => g.OrderByDescending(p => p.Date).FirstOrDefault());
            }
        }
        public static IEnumerable<IIngredient> GetLastIIngredients
        {
            get
            {
                return GetLastIngredients.Select(t => t as IIngredient).Concat(GetLastRecipes.Select(t => t as IIngredient));
            }
        }
        public static IEnumerable<Furniture> GetLastFurnitures
        {
            get
            {
                return Furnitures.GroupBy(p => p.Id).Select(g => g.OrderByDescending(p => p.Date).FirstOrDefault());
            }
        }
        public static IEnumerable<IStock> GetLastIStock
        {
            get
            {
                return GetLastIngredients.Select(t => t as IStock).Concat(GetLastFurnitures.Select(t => t as IStock));
            }
        }


        public static void Initialize()
        {
            Ingredients = DeserializeData<ObservableCollection<Ingredient>>(nameof(Ingredients)) ?? new ObservableCollection<Ingredient>();
            Recipes = DeserializeData<ObservableCollection<Recipe>>(nameof(Recipes)) ?? new ObservableCollection<Recipe>();
            Furnitures = DeserializeData<ObservableCollection<Furniture>>(nameof(Furnitures)) ?? new ObservableCollection<Furniture>();
        }

        public static void Remove(Ingredient ingredient)
        {
            while (true)
            {
                var tmp = Ingredients.FirstOrDefault(i => i.Id == ingredient.Id);
                if (tmp == null)
                {
                    break;
                }
                Ingredients.Remove(tmp);
            }
            RemoveIngredientFromRecipes(ingredient);
        }
        public static void Remove(Recipe recipe)
        {
            while(true)
            {
                var tmp = Recipes.FirstOrDefault(r => r.Id == recipe.Id);
                if(tmp == null)
                {
                    break;
                }
                Recipes.Remove(tmp);
            }
            RemoveIngredientFromRecipes(recipe);
        }
        public static void Remove(Furniture furniture)
        {
            while (true)
            {
                var tmp = Furnitures.FirstOrDefault(i => i.Id == furniture.Id);
                if (tmp == null)
                {
                    break;
                }
                Furnitures.Remove(tmp);
            }
        }

        public static void RemoveIngredientFromRecipes(IIngredient ingredient)
        {
            foreach (var recipe in Recipes.Where(r => r.RecipeIngredients.Any(i => i.Id == ingredient.Id)))
            {
                foreach (var recipeIngredient in recipe.RecipeIngredients.Where(i => i.Id == ingredient.Id))
                {
                    recipeIngredient.Id = DeletedIngredient.Id;
                }
            }
        }

        public static void SaveIngredients()
        {
            SerializeData(Ingredients, nameof(Ingredients));
        }
        public static void SaveRecipes()
        {
            SerializeData(Recipes, nameof(Recipes));
        }
        public static void SaveFurnitures()
        {
            SerializeData(Furnitures, nameof(Furnitures));
        }

        public static T DeserializeData<T>(string jsonFile)
        {
            try
            {
                string text = File.ReadAllText(Path.Combine(folder, jsonFile));
                return JsonConvert.DeserializeObject<T>(text);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing data: {ex.Message}");
                return default(T);
            }
        }
        public static void SerializeData<T>(T obj, string jsonFile)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string text = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(Path.Combine(folder, jsonFile), text);
        }
    }
}
