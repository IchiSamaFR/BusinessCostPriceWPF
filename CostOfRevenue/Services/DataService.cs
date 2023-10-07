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
        private static string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CostOfRevenue");

        public static ObservableCollection<Ingredient> Ingredients = new ObservableCollection<Ingredient>();
        public static ObservableCollection<Recipe> Recipes = new ObservableCollection<Recipe>();
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

        public static void Initialize()
        {
            Ingredients = DeserializeData<ObservableCollection<Ingredient>>(nameof(Ingredients)) ?? new ObservableCollection<Ingredient>();
            Recipes = DeserializeData<ObservableCollection<Recipe>>(nameof(Recipes)) ?? new ObservableCollection<Recipe>();
        }

        public static void SaveDatas()
        {
            SerializeData(Ingredients, nameof(Ingredients));
            SerializeData(Recipes, nameof(Recipes));
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
