using CostOfRevenue.Models;
using CostOfRevenue.Views.Pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostOfRevenue.Services
{
    public static class DataService
    {
        private static string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public static ObservableCollection<Ingredient> Ingredients = new ObservableCollection<Ingredient>();
        public static ObservableCollection<IngredientBuy> IngredientsBuy = new ObservableCollection<IngredientBuy>();
        public static ObservableCollection<Recipe> Recipes = new ObservableCollection<Recipe>();
        public static ObservableCollection<RecipeIngredient> RecipeIngredients = new ObservableCollection<RecipeIngredient>();

        public static void Initialize()
        {
            Ingredients = DeserializeData<ObservableCollection<Ingredient>>(nameof(Ingredients));
            IngredientsBuy = DeserializeData<ObservableCollection<IngredientBuy>>(nameof(IngredientsBuy));
            Recipes = DeserializeData<ObservableCollection<Recipe>>(nameof(Recipes));
            RecipeIngredients = DeserializeData<ObservableCollection<RecipeIngredient>>(nameof(RecipeIngredients));
        }

        public static void SaveDatas()
        {
            SerializeData(Ingredients, nameof(Ingredients));
            SerializeData(IngredientsBuy, nameof(IngredientsBuy));
            SerializeData(Recipes, nameof(Recipes));
            SerializeData(RecipeIngredients, nameof(RecipeIngredients));
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
            string text = JsonConvert.SerializeObject(obj);
            File.WriteAllText(Path.Combine(folder, jsonFile), text);
        }
    }
}
